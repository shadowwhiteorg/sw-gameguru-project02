using System.Collections;
using _Game.DataStructures;
using _Game.Utils;
using UnityEngine;

namespace _Game.Systems.CharacterSystem
{
    public class PlayerController : Singleton<PlayerController>
    {
        [Header("Settings")] 
        [SerializeField] private float centerMovementSpeed = 5f;
        [SerializeField] private CharacterBody body;
        

        public void MoveToPlatformCenter(float targetX)
        {
            StartCoroutine(MoveToTargetCoroutine(targetX));
        }

        private IEnumerator MoveToTargetCoroutine(float targetX)
        {
            Vector3 startPosition = body.transform.position;
            Vector3 targetPosition = new Vector3(targetX, startPosition.y, startPosition.z);
            float duration = Vector3.Distance(startPosition, targetPosition) / centerMovementSpeed;
            float elapsedTime = 0f;

            while (elapsedTime < duration)
            {
                body.transform.position = Vector3.Lerp(
                    startPosition,
                    targetPosition,
                    elapsedTime / duration
                );
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            body.transform.position = targetPosition;
        }

        private void OnEnable()
        {
            EventBus.Subscribe<OnLevelWinEvent>(e => body.PlayDanceAnimation());
            EventBus.Subscribe<OnLevelStartEvent>(e => body.PlayRunAnimation());
            EventBus.Subscribe<OnLevelFailEvent>(e => body.PlayFallAnimation());
            EventBus.Subscribe<OnLevelInitializeEvent>(e => body.PlayIdleAnimation());
        }

        private void OnDisable()
        {
            EventBus.Unsubscribe<OnLevelWinEvent>(e => body.PlayDanceAnimation());
            EventBus.Unsubscribe<OnLevelStartEvent>(e => body.PlayRunAnimation());
            EventBus.Unsubscribe<OnLevelFailEvent>(e => body.PlayFallAnimation());
            EventBus.Unsubscribe<OnLevelInitializeEvent>(e => body.PlayIdleAnimation());
        }
    }
}