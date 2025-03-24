using System;
using _Game.DataStructures;
using _Game.Systems.CharacterSystem;
using _Game.Systems.PlatformSystem;
using _Game.Utils;
using UnityEngine;
using UnityEngine.Serialization;

namespace _Game.Systems.MeshSystem
{
    public class MeshHandler : Singleton<MeshHandler>
    {
        [SerializeField] private Material platformMaterial;
        [SerializeField] private Vector3 initialPlatformSize = new Vector3(4, 1, 4);
        [SerializeField] private float failRange = 0.25f;
        [SerializeField] private float comboTolerance = 0.2f;
        [SerializeField] private float relativeSpawnPositionX = 2f;
        
        public float RelativeSpawnPositionX => initialPlatformSize.z/2;
        public float PlatformLength => initialPlatformSize.z;
        
        private int _comboCount;
        private bool _isComboActive;

        public Platform GeneratePlatform(Vector3 position, float platformWidth = 0 )
        {
            GameObject mNewPlatformObj = new GameObject("Platform");
            mNewPlatformObj.transform.position = position;
            Platform mPlatform = mNewPlatformObj.AddComponent<Platform>();
            Vector3 mPlatformSize = new Vector3(platformWidth>0 ? platformWidth: initialPlatformSize.x,initialPlatformSize.y,initialPlatformSize.z);

            GameObject mMainMeshObject = GeneratePlatformMesh(mPlatform,  mPlatformSize,  mNewPlatformObj.transform.position - RelativeSpawnPositionX*Vector3.right, true);
            
            mPlatform.Initialize(mMainMeshObject);
            
           
            
            return mPlatform;
        }

        private GameObject GeneratePlatformMesh(Platform platform, Vector3 dimensions, Vector3 position, bool isMain)
        {
            GameObject mMeshObject = new GameObject(isMain ? "MainMesh" : "SlicedMesh");
            mMeshObject.transform.position = position;
            mMeshObject.transform.SetParent(platform.transform);

            MeshFilter mMeshFilter = mMeshObject.AddComponent<MeshFilter>();
            MeshRenderer mMeshRenderer = mMeshObject.AddComponent<MeshRenderer>();
            mMeshRenderer.material = platformMaterial;

            Mesh mMesh = MeshGenerator.CreateMesh(dimensions);
            mMeshFilter.mesh = mMesh;

            if (isMain)
                platform.SetMainPart(mMeshObject);
            else
                platform.SetSlicedPart(mMeshObject);

            return mMeshFilter.gameObject;
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
            if (mainMeshSize.x <= failRange || isOutsideBounds)
            {
                Debug.Log("Fail");
                EventBus.Fire(new OnLevelFailEvent());
                return;
            }

            _isComboActive = false;
            if (slicedMeshSize.x <= comboTolerance)
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
            
            PlayerController.Instance.MoveToPlatformCenter(originalPlatform.MainPartPivot.x + originalPlatform.MainPartSize.x/2);
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
