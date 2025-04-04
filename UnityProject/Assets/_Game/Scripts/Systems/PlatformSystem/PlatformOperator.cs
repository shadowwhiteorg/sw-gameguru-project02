﻿using System.Collections;
using _Game.DataStructures;
using _Game.Interfaces;
using _Game.Scripts.Enums;
using _Game.Systems.CharacterSystem;
using _Game.Systems.Core;
using _Game.Systems.LevelSystem;
using _Game.Systems.MeshSystem;
using UnityEngine;
using _Game.Utils;

namespace _Game.Systems.PlatformSystem
{
    public class PlatformOperator : MonoBehaviour, IPlatformManager
    {
        private Platform _currentPlatform;
        private Platform _movingPlatform;
        private PlayerController _player;
        private bool _canCreatePlatform;
        private bool _hasPlatformStopped;

        // Dependencies
        private IMeshHandler _meshHandler;
        private ILevelManager _levelManager;

        public void Initialize(MeshHandler meshHandler, LevelManager levelManager)
        {
            _meshHandler = meshHandler;
            _levelManager = levelManager;
        }

        private void Update()
        {
            if (GameManager.Instance.GameState == GameState.InGame)
                CheckUnstopCondition();
        }

        private void CheckUnstopCondition()
        {
            if (_movingPlatform && _movingPlatform.transform.position.z <= _player.transform.position.z + GameConstants.PlatformChangeOffset)
                EventBus.Fire(new OnLevelFailEvent());
        }

        private void OnInitializeLevel()
        {
            _player = FindFirstObjectByType<PlayerController>();
            CreateNewPlatform();
            SetCanCreatePlatform(true);
            _hasPlatformStopped = true;
            StartCoroutine(WaitPlatformCheck());
        }

        private void OnStopPlatform()
        {
            _movingPlatform?.StopMoving();
            if (SliceOperation())
            {
                _movingPlatform?.StartFalling();
                _currentPlatform = _movingPlatform;
                _movingPlatform = null;
                _hasPlatformStopped = true;
                StartCoroutine(WaitPlatformCheck());
            }
        }

        private void CreateNewPlatform()
        {
            _currentPlatform = _meshHandler.GeneratePlatform(Vector3.forward * GameConstants.FirstPlatformOffset);
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
            Vector3 newPosition = _currentPlatform.MainPartPivot + new Vector3(0, 0, _currentPlatform.MainPartSize.z);
            _movingPlatform = _meshHandler.GeneratePlatform(newPosition, _currentPlatform.MainPartSize.x);
            _levelManager.RegisterLevelObject(_movingPlatform.gameObject);
            _movingPlatform.MoveMainPart();
        }

        public void SetCanCreatePlatform(bool canCreate)
        {
            _canCreatePlatform = canCreate;
        }

        private bool SliceOperation()
        {
            _meshHandler.SlicePlatform(_movingPlatform, _currentPlatform.MainPartPivot.x,
                _currentPlatform.MainPartPivot.x + _currentPlatform.MainPartSize.x, out var successful);
            return successful;
        }

        private IEnumerator WaitPlatformCheck()
        {
            yield return new WaitUntil(HasPlatformChanged);
            EventBus.Fire(new OnPlayerChangedPlatformEvent());
        }

        private bool HasPlatformChanged()
        {
            return _player.transform.position.z >= _currentPlatform?.transform.position.z - GameConstants.PlatformChangeOffset;
        }

        private void OnEnable()
        {
            EventBus.Subscribe<OnPlayerChangedPlatformEvent>(e => OnPlayerChangedPlatform());
            EventBus.Subscribe<OnLevelInitializeEvent>(e => OnInitializeLevel());
            EventBus.Subscribe<OnStopPlatformEvent>(e => OnStopPlatform());
        }

        private void OnDisable()
        {
            EventBus.Unsubscribe<OnLevelInitializeEvent>(e => OnInitializeLevel());
            EventBus.Unsubscribe<OnStopPlatformEvent>(e => OnStopPlatform());
            EventBus.Unsubscribe<OnPlayerChangedPlatformEvent>(e => OnPlayerChangedPlatform());
        }
    }
}