using System;
using System.Collections.Generic;
using _Game.DataStructures;
using _Game.Systems.LevelSystem;
using _Game.Systems.PlatformSystem;
using _Game.Utils;
using UnityEngine;

namespace _Game.Systems.MovementSystem
{
    public class PlatformMovement : Singleton<PlatformMovement>
    {
        [SerializeField] private List<Platform> activePlatforms = new List<Platform>();
        [SerializeField] private float removeZThreshold = -10f; // Remove when behind player
        
        private float _platformSpeed = 5f;
        private bool _isMoving = false;
        
        public float PlatformSpeed => LevelManager.Instance.CurrentLevelData.PlatformSpeed;

        void Update()
        {
            if(!_isMoving) return;
            MovePlatforms();
            CleanupPlatforms();
        }

        private void Initialize()
        {
            _isMoving = false;
            _platformSpeed = LevelManager.Instance.CurrentLevelData.PlatformSpeed;
        }

        private void MovePlatforms()
        {
            foreach (var platform in activePlatforms)
            {
                platform.transform.position += Vector3.back * (_platformSpeed * Time.deltaTime);
            }
        }

        private void SetMovement(bool start)
        {
            _isMoving = start;
        }
        
        private void CleanupPlatforms()
        {
            for (int i = activePlatforms.Count - 1; i >= 0; i--)
            {
                if (activePlatforms[i].transform.position.z < removeZThreshold)
                {
                    Destroy(activePlatforms[i].gameObject);
                    activePlatforms.RemoveAt(i);
                }
            }
        }

        public void RegisterPlatform(Platform newPlatform)
        {
            activePlatforms.Add(newPlatform);
        }

        private void OnEnable()
        {
            EventBus.Subscribe<OnLevelStartEvent>(e=> SetMovement(true));
            EventBus.Subscribe<OnLevelFailEvent>(e=> SetMovement(false));
            EventBus.Subscribe<OnLevelWinEvent>(e=>SetMovement(false));
            EventBus.Subscribe<OnLevelInitializeEvent>(e=> Initialize());
        }

        private void OnDisable()
        {
            EventBus.Unsubscribe<OnLevelStartEvent>(e=> SetMovement(true));
            EventBus.Unsubscribe<OnLevelFailEvent>(e=> SetMovement(false));
            EventBus.Unsubscribe<OnLevelWinEvent>(e=>SetMovement(false));
            EventBus.Unsubscribe<OnLevelInitializeEvent>(e=> Initialize());

        }
    }
}