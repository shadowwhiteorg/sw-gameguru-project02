using _Game.DataStructures;
using _Game.Utils;
using _Game.Interfaces;

namespace _Game.Systems.MovementSystem
{
    using UnityEngine;

    public class ParallaxObject : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private float parallaxSpeedMultiplier = 0.5f;

        private IPlatformMovement _platformMovement;
        private ILevelManager _levelManager;
        private bool _isMoving;

        public void Initialize(ILevelManager levelManager)
        {
            _levelManager = levelManager;
        }

        private void Update()
        {
            if (_isMoving)
            {
                transform.position += Vector3.back * (
                    _levelManager.CurrentLevelData.PlatformSpeed * 
                    parallaxSpeedMultiplier * 
                    Time.deltaTime
                );
            }
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