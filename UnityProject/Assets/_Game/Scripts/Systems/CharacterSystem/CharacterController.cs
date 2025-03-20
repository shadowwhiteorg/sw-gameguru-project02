using System.Collections;
using System.Collections.Generic;
using _Game.Utils;
using UnityEngine;

namespace _Game.Systems.CharacterSystem
{
    public class CharacterController : Singleton<CharacterController>
    {
        [SerializeField] private CharacterBody body;
        [SerializeField] private float centerMovementSpeed;

        public void MoveToPlatformCenter(float targetX)
        {
            StartCoroutine(MoveToTargetCoroutine(targetX, centerMovementSpeed));
        }

        private IEnumerator MoveToTargetCoroutine(float targetX, float speed)
        {
            Vector3 startPosition = body.transform.position;
            Vector3 targetPosition = new Vector3(targetX, startPosition.y, startPosition.z);
            float elapsedTime = 0f;
            float duration = Vector3.Distance(startPosition, targetPosition) / speed;

            while (elapsedTime < duration)
            {
                body.transform.position = Vector3.Lerp(startPosition, targetPosition, elapsedTime / duration);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            body.transform.position = targetPosition;
        }
    }
}