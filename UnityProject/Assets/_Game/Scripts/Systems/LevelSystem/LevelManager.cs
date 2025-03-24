using System.Collections;
using System.Collections.Generic;
using _Game.DataStructures;
using _Game.Interfaces;
using _Game.Systems.CharacterSystem;
using _Game.Systems.MeshSystem;
using _Game.Systems.MovementSystem;
using _Game.Utils;
using UnityEngine;

namespace _Game.Systems.LevelSystem
{
    public class LevelManager : MonoBehaviour, ILevelManager
{
    [Header("Settings")]
    [SerializeField] private LevelDataCatalog levelDataCatalog;
    [SerializeField] private GameObject finalPlatformPrefab;

    private int _currentStep;
    private GameObject _finalPlatform;
    private List<GameObject> _levelObjects = new List<GameObject>();
    private IPlatformManager _platformManager;
    private PlayerController _playerController;
    private MeshHandler _meshHandler;

    public int CurrentLevel => PlayerPrefs.GetInt(GameConstants.PlayerPrefsLevel, 1);
    public LevelData CurrentLevelData => levelDataCatalog.Levels[CurrentLevel % levelDataCatalog.Levels.Count];
    public float PlatformLength => _meshHandler.PlatformLength;

    public void Initialize(IPlatformManager platformManager, PlayerController playerController, MeshHandler meshHandler)
    {
        _platformManager = platformManager;
        _playerController = playerController;
        _meshHandler = meshHandler;
        ResetLevel();
    }

    public void RegisterLevelObject(GameObject levelObject)
    {
        _levelObjects.Add(levelObject);
    }

    public void PlatformStopped()
    {
        _currentStep++;
        if (_currentStep >= CurrentLevelData.NumberOfPlatforms)
        {
            _platformManager.SetCanCreatePlatform(false);
            StartCoroutine(CheckFinalLine());
        }
    }

    private IEnumerator CheckFinalLine()
    {
        yield return new WaitUntil(() => 
            _finalPlatform.transform.position.z <= 
            _playerController.transform.position.z - GameConstants.FinalPlatformDistance);
        
        EventBus.Fire(new OnLevelWinEvent());
    }

    private void ResetLevel()
    {
        ClearPlatforms();
        SpawnFinalPlatform();
        _currentStep = 1;
    }

    private void SpawnFinalPlatform()
    {
        _finalPlatform = Instantiate(finalPlatformPrefab, Vector3.zero, Quaternion.identity);
        _finalPlatform.GetComponent<ParallaxObject>().Initialize(this);
        _levelObjects.Add(_finalPlatform);
        float finalZ = CurrentLevelData.NumberOfPlatforms * PlatformLength + 
                      GameConstants.FirstPlatformOffset;
        _finalPlatform = Instantiate(finalPlatformPrefab, 
            Vector3.forward * finalZ, 
            Quaternion.identity
        );
        _finalPlatform.GetComponent<ParallaxObject>().Initialize(this);
        RegisterLevelObject(_finalPlatform);
    }

    private void ClearPlatforms()
    {
        foreach (var platform in _levelObjects)
        {
            if (platform != null) Destroy(platform);
        }
        _levelObjects.Clear();
    }

    private void UpdateCurrentLevel()
    {
        PlayerPrefs.SetInt(GameConstants.PlayerPrefsLevel, CurrentLevel + 1);
    }

    private void OnEnable()
    {
        EventBus.Subscribe<OnLevelInitializeEvent>(e => ResetLevel());
        EventBus.Subscribe<OnLevelWinEvent>(e => UpdateCurrentLevel());
        EventBus.Subscribe<OnStopPlatformEvent>(e => PlatformStopped());
    }

    private void OnDisable()
    {
        EventBus.Unsubscribe<OnLevelInitializeEvent>(e => ResetLevel());
        EventBus.Unsubscribe<OnLevelWinEvent>(e => UpdateCurrentLevel());
        EventBus.Unsubscribe<OnStopPlatformEvent>(e => PlatformStopped());
    }
}
}