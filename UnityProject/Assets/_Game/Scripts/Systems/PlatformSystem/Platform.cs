﻿using System.Collections;
using System.Collections.Generic;
using _Game.Systems.MeshSystem;
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
        }
        
        public void MoveMainPart()
        {
            _moveCoroutine = StartCoroutine(MoveToX());
        }

        private IEnumerator MoveToX()
        {
            float duration = 1f; // Adjust as needed
            Vector3 startPos = _mainPart.transform.position;
            Vector3 endPos = new Vector3(startPos.x + PlatformMeshHandler.Instance.RelativeSpawnPositionX*2, startPos.y, startPos.z);

            while (true) // Infinite loop for ping-pong effect
            {
                // Move from startPos to endPos
                float elapsedTime = 0f;
                while (elapsedTime < duration)
                {
                    elapsedTime += Time.deltaTime;
                    _mainPart.transform.position = Vector3.Lerp(startPos, endPos, elapsedTime / duration);
                    yield return null;
                }
                _mainPart.transform.position = endPos;

                // Move from endPos back to startPos
                elapsedTime = 0f;
                while (elapsedTime < duration)
                {
                    elapsedTime += Time.deltaTime;
                    _mainPart.transform.position = Vector3.Lerp(endPos, startPos, elapsedTime / duration);
                    yield return null;
                }
                _mainPart.transform.position = startPos;
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
        
        public void StartMoving()
        {
            _isMoving = true;
            _startPosition = transform.position - Vector3.right * movementRange;
        }

        public void StopMoving()
        {
            _isMoving = false;
            StopCoroutine(_moveCoroutine);
        }
        
        private void Update()
        {
            if (_isMoving)
            {
                float x = _startPosition.x + Mathf.PingPong(Time.time * _speed, PlatformMeshHandler.Instance.RelativeSpawnPositionX*2);
                transform.position = new Vector3(x, transform.position.y, transform.position.z);
            }
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