using System;
using _Game.DataStructures;
using _Game.Utils;
using UnityEngine;

namespace _Game.Systems.MovementSystem
{
    public class ParallaxObject : MonoBehaviour
    {
        [SerializeField] private float parallaxSpeedMultiplier = 0.5f;

        private bool _isMoving;
        
        private void SetMovement(bool start)
        {
            _isMoving = start;
        }
        
        void Update()
        {
            if (_isMoving) 
                transform.position += Vector3.back * (PlatformMovement.Instance.PlatformSpeed * parallaxSpeedMultiplier * Time.deltaTime);
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