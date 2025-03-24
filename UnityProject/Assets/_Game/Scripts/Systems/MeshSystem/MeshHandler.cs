using _Game.DataStructures;
using _Game.Interfaces;
using _Game.Systems.CharacterSystem;
using _Game.Systems.PlatformSystem;
using _Game.Utils;
using UnityEngine;

namespace _Game.Systems.MeshSystem
{
    public class MeshHandler : MonoBehaviour, IMeshHandler
    {
        [Header("Settings")] [SerializeField] private Material platformMaterial;
        [SerializeField] private Vector3 initialPlatformSize = new Vector3(4, 1, 4);
        [SerializeField] private float failRange = 0.25f;
        [SerializeField] private float comboTolerance = 0.2f;

        private int _comboCount;
        private bool _isComboActive;
        private PlayerController _playerController;
        private IPlatformMovement _platformMovement;
        public float PlatformLength => initialPlatformSize.z;
        public float RelativeSpawnPositionX => initialPlatformSize.z / 2;

        public void Initialize(PlayerController playerController, IPlatformMovement platformMovement)
        {
            _playerController = playerController;
            _platformMovement = platformMovement;
        }

        public Platform GeneratePlatform(Vector3 position, float platformWidth = 0)
        {
            var platformObj = new GameObject("Platform");
            platformObj.transform.position = position;
            var platform = platformObj.AddComponent<Platform>();
            _platformMovement.RegisterPlatform(platform);
            var platformSize = new Vector3(
                platformWidth > 0 ? platformWidth : initialPlatformSize.x,
                initialPlatformSize.y,
                initialPlatformSize.z
            );

            var mainMesh = GeneratePlatformMesh(platform, platformSize,
                platformObj.transform.position - RelativeSpawnPositionX * Vector3.right, true);

            platform.Initialize(mainMesh, this);
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
            renderer.material = platformMaterial;

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
            if (leftBound >= rightBound) return;

            float originalLeft = originalPlatform.MainPartPivot.x;
            float originalRight = originalLeft + originalPlatform.MainPartSize.x;

            bool isSlicingFromRight = rightBound < originalRight;

            var (mainMeshPosition, mainMeshSize, slicedMeshPosition, slicedMeshSize) = CalculateMeshPositionsAndSizes(
                originalPlatform, leftBound, rightBound, originalLeft, originalRight, isSlicingFromRight);

            originalPlatform.MainPart.SetActive(false);
            if (IsFailCondition(mainMeshSize.x, leftBound, rightBound, originalLeft, originalRight))
            {
                EventBus.Fire(new OnLevelFailEvent());
                return;
            }

            HandleCombo(slicedMeshSize.x);

            GameObject mainMesh = GeneratePlatformMesh(originalPlatform, mainMeshSize, mainMeshPosition, true);
            GameObject slicedMesh = GeneratePlatformMesh(originalPlatform, slicedMeshSize, slicedMeshPosition, false);

            isSuccessful = true;
            originalPlatform.SetMainPart(mainMesh);
            originalPlatform.SetSlicedPart(slicedMesh);

            _playerController.MoveToPlatformCenter(originalPlatform.MainPartPivot.x +
                                                   originalPlatform.MainPartSize.x / 2);
        }

        private (Vector3 mainMeshPosition, Vector3 mainMeshSize, Vector3 slicedMeshPosition, Vector3 slicedMeshSize)
            CalculateMeshPositionsAndSizes(Platform originalPlatform, float leftBound, float rightBound,
                float originalLeft, float originalRight, bool isSlicingFromRight)
        {
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

            return (mainMeshPosition, mainMeshSize, slicedMeshPosition, slicedMeshSize);
        }

        private bool IsFailCondition(float mainMeshSizeX, float leftBound, float rightBound, float originalLeft,
            float originalRight)
        {
            bool isOutsideBounds = leftBound > originalRight || rightBound < originalLeft;
            return mainMeshSizeX <= failRange || isOutsideBounds;
        }

        private void HandleCombo(float slicedMeshSizeX)
        {
            _isComboActive = false;
            if (slicedMeshSizeX <= comboTolerance)
            {
                _isComboActive = true;
                _comboCount++;
                EventBus.Fire(new OnComboEvent(_comboCount));
            }

            if (!_isComboActive) _comboCount = 1;
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
