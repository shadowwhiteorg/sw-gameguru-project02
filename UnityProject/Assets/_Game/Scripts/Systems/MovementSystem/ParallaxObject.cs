using System;
using _Game.DataStructures;
using _Game.Systems.LevelSystem;
using _Game.Utils;
using UnityEngine;

namespace _Game.Systems.MovementSystem
{
    public class ParallaxObject : MonoBehaviour
    {
        [SerializeField] private float parallaxSpeedMultiplier = 0.5f;
        private bool _isMoving;
        private PlatformMovement _platformMovement;
        private LevelManager _levelManager;
        
        public void Initialize(PlatformMovement platformMovement, LevelManager levelManager)
        {
            _platformMovement = platformMovement;
            _levelManager = levelManager;
        }
        
        private void SetMovement(bool start)
        {
            _isMoving = start;
        }
        
        void Update()
        {
            // if (_isMoving) 
            //     transform.position += Vector3.back * (_levelManager.CurrentLevelData.PlatformSpeed* parallaxSpeedMultiplier * Time.deltaTime);
        }

        private void OnEnable()
        {
            EventBus.Subscribe<OnLevelStartEvent>(e=> SetMovement(true));
            EventBus.Subscribe<OnLevelFailEvent>(e=> SetMovement(false));
            EventBus.Subscribe<OnLevelWinEvent>(e=>SetMovement(false));
        }

        private void OnDisable()
        {
            EventBus.Unsubscribe<OnLevelStartEvent>(e=> SetMovement(true));
            EventBus.Unsubscribe<OnLevelFailEvent>(e=> SetMovement(false));
            EventBus.Unsubscribe<OnLevelWinEvent>(e=>SetMovement(false));
        }
    }
}