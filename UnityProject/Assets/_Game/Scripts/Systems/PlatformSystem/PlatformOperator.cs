using System;
using _Game.DataStructures;
using _Game.Systems.CharacterSystem;
using _Game.Systems.LevelSystem;
using _Game.Systems.MeshSystem;
using _Game.Systems.PlatformSystem;
using UnityEngine;
using _Game.Utils;

namespace _Game.Systems.PlatformSystem
{
    public class PlatformOperator : Singleton<PlatformOperator>
    {
        private Platform _currentPlatform;
        private Platform _movingPlatform;
        private bool _canCreatePlatform;
        private PlayerController _player;

        private void OnStopPlatform()
        {
            _movingPlatform.StopMoving();
            if(!SliceOperation()) return;
            _movingPlatform.StartFalling();
            _currentPlatform = _movingPlatform;
            if(_canCreatePlatform)
                CreateNewMovingPlatform();
        }
        
        private void OnInitializeLevel()
        {
            _player = FindFirstObjectByType<PlayerController>();
            CreateNewPlatform();
            CreateNewMovingPlatform();
            SetCanCreatePlatform(true);
        }

        private void CreateNewPlatform()
        {
            // _currentPlatform = MeshHandler.Instance.GeneratePlatform(  Vector3.forward * GameConstants.FirstPlatformOffset + (_currentPlatform ? _currentPlatform.MainPartPivot : Vector3.zero));
            _currentPlatform = MeshHandler.Instance.GeneratePlatform(Vector3.forward * GameConstants.FirstPlatformOffset);
            LevelManager.Instance.RegisterLevelObject(_currentPlatform.gameObject);
        }
        
        private void CreateNewMovingPlatform()
        {
            Vector3 newPosition = _currentPlatform.MainPartPivot + new Vector3(0, 0,_currentPlatform.MainPartSize.z);
            _movingPlatform = MeshHandler.Instance.GeneratePlatform( newPosition, _currentPlatform.MainPartSize.x);
            LevelManager.Instance.RegisterLevelObject(_movingPlatform.gameObject);
            _movingPlatform.MoveMainPart();
        }

        public void SetCanCreatePlatform(bool canCreate)
        {
            _canCreatePlatform = canCreate;
        }

        private bool SliceOperation()
        {
            MeshHandler.Instance.SlicePlatform(_movingPlatform, _currentPlatform.MainPartPivot.x,
                _currentPlatform.MainPartPivot.x + _currentPlatform.MainPartSize.x, out var successful);
            return successful;
        }
        
        private void OnEnable()
        {
            EventBus.Subscribe<OnLevelInitializeEvent>(e => OnInitializeLevel());
            EventBus.Subscribe<OnStopPlatformEvent>(e => OnStopPlatform());
        }

        private void OnDisable()
        {
            EventBus.Unsubscribe<OnLevelInitializeEvent>(e => OnInitializeLevel());
            EventBus.Unsubscribe<OnStopPlatformEvent>(e => OnStopPlatform());
        }
        
    }
}