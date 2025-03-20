using System.Collections;
using System.Collections.Generic;
using _Game.Systems.MeshSystem;
using _Game.Systems.MovementSystem;
using UnityEngine;
using UnityEngine.Serialization;

namespace _Game.Systems.PlatformSystem
{
    public class Platform : MonoBehaviour
    {
        private Vector3 _dimensions;
        private Material _platformMaterial;
        private GameObject _mainPart;
        private GameObject _slicedPart;
        private bool _isMoving;
        private float _speed = 3f;
        [SerializeField]private float movementRange = 5f;
        [SerializeField] private float fallSpeed = 2f;
        [SerializeField] private float destroyHeight = -5f;
        private Vector3 _startPosition;
        private Coroutine _moveCoroutine;

        public GameObject MainPart => _mainPart;
        public Vector3 Dimensions => _dimensions;
        public Material PlatformMaterial => _platformMaterial;
        public Vector3 MainPartCenterPosition => _mainPart.GetComponent<MeshRenderer>().bounds.center;
        public Vector3 MainPartSize => _mainPart.GetComponent<MeshRenderer>().bounds.size;

        public Vector3 MainPartPivot => new Vector3(MainPartCenterPosition.x - MainPartSize.x / 2,
            MainPartCenterPosition.y - MainPartSize.y / 2, MainPartCenterPosition.z - MainPartSize.z / 2);
        
        public void Initialize(GameObject mainPart)
        {
            _mainPart = mainPart;
            _isMoving = false;
            PlatformMovement.Instance.RegisterPlatform(this);
        }
        
        public void MoveMainPart()
        {
            _moveCoroutine = StartCoroutine(MoveToX());
        }

        private IEnumerator MoveToX()
        {
            float duration = 1f;
            Vector3 startPos = _mainPart.transform.localPosition;
            Vector3 endPos = new Vector3(startPos.x + MeshHandler.Instance.RelativeSpawnPositionX*2, startPos.y, startPos.z);

            while (true) // Infinite loop for ping-pong effect
            {
                float elapsedTime = 0f;
                while (elapsedTime < duration)
                {
                    elapsedTime += Time.deltaTime;
                    _mainPart.transform.localPosition = Vector3.Lerp(startPos, endPos, elapsedTime / duration);
                    yield return null;
                }
                _mainPart.transform.localPosition = endPos;

                elapsedTime = 0f;
                while (elapsedTime < duration)
                {
                    elapsedTime += Time.deltaTime;
                    _mainPart.transform.localPosition = Vector3.Lerp(endPos, startPos, elapsedTime / duration);
                    yield return null;
                }
                _mainPart.transform.localPosition = startPos;
            }
        }
        
        public void SetSlicedPart(GameObject slicedPart)
        {
            _slicedPart = slicedPart;
        }
        
        public void SetMainPart(GameObject mainPart)
        {
            _mainPart = mainPart;
        }
        public void StopMoving()
        {
            _isMoving = false;
            StopCoroutine(_moveCoroutine);
        }

        public void StartFalling()
        {
            transform.SetParent(null);
            StartCoroutine(FallDown());
        }

        private IEnumerator FallDown()
        {
            while (_slicedPart.transform.position.y > destroyHeight)
            {
                _slicedPart.transform.position += Vector3.down * (fallSpeed * Time.deltaTime);
                yield return null;
            }

            Destroy(_slicedPart);
        }
    }
}