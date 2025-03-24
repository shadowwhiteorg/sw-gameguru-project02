using System.Collections;
using _Game.DataStructures;
using _Game.Interfaces;
using _Game.Utils;
using UnityEngine;

namespace _Game.Systems.CharacterSystem
{
    public class PlayerController : Singleton<PlayerController>
    {
        [Header("Settings")] [SerializeField] private float _centerMovementSpeed = 5f;
        [SerializeField] private CharacterBody _body;

        private IPlatformManager _platformManager;

        public void Initialize(IPlatformManager platformManager)
        {
            _platformManager = platformManager;
        }

        public void MoveToPlatformCenter(float targetX)
        {
            StartCoroutine(MoveToTargetCoroutine(targetX));
        }

        private IEnumerator MoveToTargetCoroutine(float targetX)
        {
            Vector3 startPosition = _body.transform.position;
            Vector3 targetPosition = new Vector3(targetX, startPosition.y, startPosition.z);
            float duration = Vector3.Distance(startPosition, targetPosition) / _centerMovementSpeed;
            float elapsedTime = 0f;

            while (elapsedTime < duration)
            {
                _body.transform.position = Vector3.Lerp(
                    startPosition,
                    targetPosition,
                    elapsedTime / duration
                );
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            _body.transform.position = targetPosition;
        }

        private void OnEnable()
        {
            EventBus.Subscribe<OnLevelWinEvent>(e => _body.PlayDanceAnimation());
            EventBus.Subscribe<OnLevelStartEvent>(e => _body.PlayRunAnimation());
            EventBus.Subscribe<OnLevelFailEvent>(e => _body.PlayFallAnimation());
            EventBus.Subscribe<OnLevelInitializeEvent>(e => _body.PlayIdleAnimation());
        }

        private void OnDisable()
        {
            EventBus.Unsubscribe<OnLevelWinEvent>(e => _body.PlayDanceAnimation());
            EventBus.Unsubscribe<OnLevelStartEvent>(e => _body.PlayRunAnimation());
            EventBus.Unsubscribe<OnLevelFailEvent>(e => _body.PlayFallAnimation());
            EventBus.Unsubscribe<OnLevelInitializeEvent>(e => _body.PlayIdleAnimation());
        }
    }
}