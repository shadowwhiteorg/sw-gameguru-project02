using System;
using System.Collections.Generic;
using _Game.DataStructures;
using _Game.Interfaces;
using _Game.Systems.LevelSystem;
using _Game.Systems.PlatformSystem;
using _Game.Utils;
using UnityEngine;
using UnityEngine.Serialization;

namespace _Game.Systems.MovementSystem
{
    public class PlatformMovement : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private float removeZThreshold = -10f;
    
        // Dependencies
        private ILevelManager _levelManager;
        private List<Platform> _activePlatforms = new List<Platform>();
        private bool _isMoving;

        public void Initialize(ILevelManager levelManager)
        {
            _levelManager = levelManager;
        }
        public float PlatformSpeed => _levelManager.CurrentLevelData.PlatformSpeed;
        private void Update()
        {
            if (!_isMoving) return;
        
            MovePlatforms();
            CleanupPlatforms();
        }

        private void MovePlatforms()
        {
            foreach (var platform in _activePlatforms)
            {
                platform.transform.position += 
                    Vector3.back * (PlatformSpeed * Time.deltaTime);
            }
        }

        private void CleanupPlatforms()
        {
            for (int i = _activePlatforms.Count - 1; i >= 0; i--)
            {
                if (_activePlatforms[i].transform.position.z < removeZThreshold)
                {
                    Destroy(_activePlatforms[i].gameObject);
                    _activePlatforms.RemoveAt(i);
                }
            }
        }

        public void RegisterPlatform(Platform newPlatform)
        {
            _activePlatforms.Add(newPlatform);
        }

        private void SetMovement(bool start) => _isMoving = start;

        private void OnEnable()
        {
            EventBus.Subscribe<OnLevelStartEvent>(e => SetMovement(true));
            EventBus.Subscribe<OnLevelFailEvent>(e => SetMovement(false));
            EventBus.Subscribe<OnLevelWinEvent>(e => SetMovement(false));
        }

        private void OnDisable()
        {
            EventBus.Unsubscribe<OnLevelStartEvent>(e => SetMovement(true));
            EventBus.Unsubscribe<OnLevelFailEvent>(e => SetMovement(false));
            EventBus.Unsubscribe<OnLevelWinEvent>(e => SetMovement(false));
        }
    }
}