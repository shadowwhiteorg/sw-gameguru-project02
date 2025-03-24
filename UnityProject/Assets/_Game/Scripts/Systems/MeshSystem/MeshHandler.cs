using System;
using _Game.DataStructures;
using _Game.Interfaces;
using _Game.Systems.CharacterSystem;
using _Game.Systems.PlatformSystem;
using _Game.Utils;
using UnityEngine;
using UnityEngine.Serialization;

namespace _Game.Systems.MeshSystem
{
    public class MeshHandler : MonoBehaviour, IMeshHandler
    {
        [Header("Settings")]
        [SerializeField] private Material _platformMaterial;
        [SerializeField] private Vector3 _initialPlatformSize = new Vector3(4, 1, 4);
        [SerializeField] private float _failRange = 0.25f;
        [SerializeField] private float _comboTolerance = 0.2f;
        
    
        private int _comboCount;
        private bool _isComboActive;
        private PlayerController _playerController;
        public float PlatformLength => _initialPlatformSize.z;
        public float RelativeSpawnPositionX => _initialPlatformSize.z / 2;

        public void Initialize(PlayerController playerController)
        {
            _playerController = playerController;
        }
        public Platform GeneratePlatform(Vector3 position, float platformWidth = 0)
        {
            var platformObj = new GameObject("Platform");
            platformObj.transform.position = position;
            var platform = platformObj.AddComponent<Platform>();
        
            var platformSize = new Vector3(
                platformWidth > 0 ? platformWidth : _initialPlatformSize.x,
                _initialPlatformSize.y,
                _initialPlatformSize.z
            );

            var mainMesh = GeneratePlatformMesh(platform, platformSize, 
                platformObj.transform.position - RelativeSpawnPositionX * Vector3.right, true);
        
            platform.Initialize(mainMesh);
            return platform;
        }

        private GameObject GeneratePlatformMesh(Platform platform, Vector3 dimensions, 
            Vector3 position, bool isMain)
        {
            var meshObj = new GameObject(isMain ? "MainMesh" : "SlicedMesh");
            meshObj.transform.position = position;
            meshObj.transform.SetParent(platform.transform);

            var filter = meshObj.AddComponent<MeshFilter>();
            var renderer = meshObj.AddComponent<MeshRenderer>();
            renderer.material = _platformMaterial;

            filter.mesh = MeshGenerator.CreateMesh(dimensions);

            if (isMain)
                platform.SetMainPart(meshObj);
            else
                platform.SetSlicedPart(meshObj);

            return meshObj;
        }

        public void SlicePlatform(Platform originalPlatform, float leftBound, float rightBound, out bool isSuccessful)
        {
            isSuccessful = false;
            if (leftBound >= rightBound ) return;
    
            float originalLeft = originalPlatform.MainPartPivot.x;
            float originalRight = originalLeft + originalPlatform.MainPartSize.x;

            bool isSlicingFromRight = rightBound < originalRight;

            Vector3 mainMeshPosition, slicedMeshPosition;
            Vector3 mainMeshSize, slicedMeshSize;

            if (isSlicingFromRight)
            {
                mainMeshPosition = originalPlatform.MainPartPivot;
                mainMeshSize = new Vector3(rightBound - originalLeft, originalPlatform.MainPartSize.y,
                    originalPlatform.MainPartSize.z);

                slicedMeshPosition = new Vector3(rightBound, originalPlatform.MainPartPivot.y,
                    originalPlatform.MainPartPivot.z);
                slicedMeshSize = new Vector3(originalRight - rightBound, originalPlatform.MainPartSize.y,
                    originalPlatform.MainPartSize.z);
            }
            else
            {
                mainMeshPosition = new Vector3(leftBound, originalPlatform.MainPartPivot.y,
                    originalPlatform.MainPartPivot.z);
                mainMeshSize = new Vector3(originalRight - leftBound, originalPlatform.MainPartSize.y,
                    originalPlatform.MainPartSize.z);

                slicedMeshPosition = originalPlatform.MainPartPivot;
                slicedMeshSize = new Vector3(leftBound - originalLeft, originalPlatform.MainPartSize.y,
                    originalPlatform.MainPartSize.z);
            }

            
              
            originalPlatform.MainPart.SetActive(false);
            bool isOutsideBounds = leftBound > originalRight || rightBound < originalLeft;
            if (mainMeshSize.x <= _failRange || isOutsideBounds)
            {
                EventBus.Fire(new OnLevelFailEvent());
                return;
            }

            _isComboActive = false;
            if (slicedMeshSize.x <= _comboTolerance)
            {
                _isComboActive = true;
                _comboCount++;
                EventBus.Fire(new OnComboEvent(_comboCount));
            }
            if (!_isComboActive) _comboCount = 1;
                

            GameObject mainMesh = GeneratePlatformMesh(originalPlatform, mainMeshSize, mainMeshPosition, true);
            GameObject slicedMesh = GeneratePlatformMesh(originalPlatform, slicedMeshSize, slicedMeshPosition, false);

            isSuccessful = true;
            originalPlatform.SetMainPart(mainMesh);
            originalPlatform.SetSlicedPart(slicedMesh);
            
            _playerController.MoveToPlatformCenter(originalPlatform.MainPartPivot.x + originalPlatform.MainPartSize.x/2);
        }

        private void OnLevelInitialized()
        {
            _comboCount = 1;
        }

        private void OnEnable()
        {
            EventBus.Subscribe<OnLevelInitializeEvent>(e => OnLevelInitialized());
        }
        
        private void OnDisable()
        {
            EventBus.Unsubscribe<OnLevelInitializeEvent>(e => OnLevelInitialized());
        }
    }
}
