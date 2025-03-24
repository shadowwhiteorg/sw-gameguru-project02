using System.Collections;
using _Game.DataStructures;
using _Game.Interfaces;
using _Game.Scripts.Enums;
using _Game.Systems.CharacterSystem;
using _Game.Systems.Core;
using _Game.Systems.LevelSystem;
using _Game.Systems.MeshSystem;
using _Game.Systems.MovementSystem;
using UnityEngine;
using _Game.Utils;

namespace _Game.Systems.PlatformSystem
{
    public class PlatformOperator : MonoBehaviour, IPlatformManager
{
    [Header("Settings")]
    [SerializeField] private float _platformChangeOffset = 2f;
    
    // Dependencies
    private IMeshHandler _meshHandler;
    private ILevelManager _levelManager;
    private PlatformMovement _platformMovement;
    private PlayerController _playerController;
    
    // Runtime state
    private Platform _currentPlatform;
    private Platform _movingPlatform;
    private bool _canCreatePlatform;
    private bool _hasPlatformStopped;

    public void Initialize(
        IMeshHandler meshHandler,
        ILevelManager levelManager,
        PlatformMovement platformMovement,
        PlayerController playerController)
    {
        _meshHandler = meshHandler;
        _levelManager = levelManager;
        _platformMovement = platformMovement;
        _playerController = playerController;
    }

    public void SetCanCreatePlatform(bool canCreate) => _canCreatePlatform = canCreate;

    private void OnEnable()
    {
        EventBus.Subscribe<OnPlayerChangedPlatformEvent>(e=>OnPlayerChangedPlatform());
        EventBus.Subscribe<OnLevelInitializeEvent>(e=>OnInitializeLevel());
        EventBus.Subscribe<OnStopPlatformEvent>(e=>OnStopPlatform());
    }

    private void OnDisable()
    {
        EventBus.Unsubscribe<OnPlayerChangedPlatformEvent>(e=>OnPlayerChangedPlatform());
        EventBus.Unsubscribe<OnLevelInitializeEvent>(e=>OnInitializeLevel());
        EventBus.Unsubscribe<OnStopPlatformEvent>(e=>OnStopPlatform());
    }

    private void OnInitializeLevel()
    {
        CreateNewPlatform();
        SetCanCreatePlatform(true);
        _hasPlatformStopped = true;
        StartCoroutine(WaitPlatformCheck());
    }

    private IEnumerator WaitPlatformCheck()
    {
        yield return new WaitUntil(HasPlatformChanged);
        EventBus.Fire(new OnPlayerChangedPlatformEvent());
    }

    private bool HasPlatformChanged()
    {
        return _playerController.transform.position.z >= 
               _currentPlatform.transform.position.z - _platformChangeOffset;
    }

    private void OnStopPlatform()
    {
        if (_movingPlatform == null) return;
        
        _movingPlatform.StopMoving();
        if (!SliceOperation()) return;
        
        _movingPlatform.StartFalling();
        _currentPlatform = _movingPlatform;
        _movingPlatform = null;
        _hasPlatformStopped = true;
        StartCoroutine(WaitPlatformCheck());
    }

    private bool SliceOperation()
    {
        _meshHandler.SlicePlatform(
            _movingPlatform, 
            _currentPlatform.MainPartPivot.x,
            _currentPlatform.MainPartPivot.x + _currentPlatform.MainPartSize.x, 
            out var successful
        );
        return successful;
    }

    private void CreateNewPlatform()
    {
        _currentPlatform = _meshHandler.GeneratePlatform(
            Vector3.forward * GameConstants.FirstPlatformOffset
        );
        _levelManager.RegisterLevelObject(_currentPlatform.gameObject);
    }

    private void OnPlayerChangedPlatform()
    {
        if (!_hasPlatformStopped)
        {
            EventBus.Fire(new OnLevelFailEvent());
            return;
        }
        CreateNewMovingPlatform();
    }

    private void CreateNewMovingPlatform()
    {
        if (!_canCreatePlatform) return;
        
        _hasPlatformStopped = false;
        Vector3 newPosition = _currentPlatform.MainPartPivot + 
                            new Vector3(0, 0, _currentPlatform.MainPartSize.z);
        
        _movingPlatform = _meshHandler.GeneratePlatform(
            newPosition, 
            _currentPlatform.MainPartSize.x
        );
        
        _levelManager.RegisterLevelObject(_movingPlatform.gameObject);
        _movingPlatform.MoveMainPart();
    }
}
}