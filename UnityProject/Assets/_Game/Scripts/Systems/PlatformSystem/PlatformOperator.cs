using System;
using System.Collections;
using _Game.DataStructures;
using _Game.Scripts.Enums;
using _Game.Systems.CharacterSystem;
using _Game.Systems.Core;
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
        private Platform _lastPlatform;
        private Platform _movingPlatform;
        private bool _canCreatePlatform;
        private PlayerController _player;
        private bool _hasPlatformStopped;

        private IEnumerator WaitPlatformCheck()
        {
            yield return new WaitUntil(HasPlatformChanged);
            EventBus.Fire(new OnPlayerChangedPlatformEvent());
        }

        private void Update()
        {
            if(GameManager.Instance.GameState == GameState.InGame)
                CheckUnstopCondition();
                
        }

        private bool HasPlatformChanged()
        {
            return _player.transform.position.z>= _currentPlatform?.transform.position.z- GameConstants.PlatformChangeOffset;
        }

        private void CheckUnstopCondition()
        {
            if(!_movingPlatform)return;
            if(_movingPlatform.transform.position.z <= _player.transform.position.z + GameConstants.PlatformChangeOffset)
                EventBus.Fire(new OnLevelFailEvent());
        }
        
        private void OnInitializeLevel()
        {
            _player = FindFirstObjectByType<PlayerController>();
            CreateNewPlatform();
            SetCanCreatePlatform(true);
            // CreateNewMovingPlatform();
            _hasPlatformStopped = true;
            StartCoroutine(WaitPlatformCheck());
        }
        
        private void OnStopPlatform()
        {
            _movingPlatform?.StopMoving();
            if(!SliceOperation()) return;
            _movingPlatform?.StartFalling();
            _currentPlatform = _movingPlatform;
            _movingPlatform = null;
            _hasPlatformStopped = true;
            StartCoroutine(WaitPlatformCheck());
        }

        private void CreateNewPlatform()
        {
            _currentPlatform = MeshHandler.Instance.GeneratePlatform(Vector3.forward * GameConstants.FirstPlatformOffset);
            LevelManager.Instance.RegisterLevelObject(_currentPlatform.gameObject);
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
            if(!_canCreatePlatform) return;
            _hasPlatformStopped = false;
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
            EventBus.Subscribe<OnPlayerChangedPlatformEvent>(e => OnPlayerChangedPlatform());
        }

        private void OnDisable()
        {
            EventBus.Unsubscribe<OnLevelInitializeEvent>(e => OnInitializeLevel());
            EventBus.Unsubscribe<OnStopPlatformEvent>(e => OnStopPlatform());
            EventBus.Unsubscribe<OnPlayerChangedPlatformEvent>(e => OnPlayerChangedPlatform());

        }
        
    }
}