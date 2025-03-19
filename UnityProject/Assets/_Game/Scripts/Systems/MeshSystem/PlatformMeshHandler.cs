using _Game.Systems.Core;
using _Game.Systems.PlatformSystem;
using _Game.Utils;
using UnityEngine;

namespace _Game.Systems.MeshSystem
{
    public class PlatformMeshHandler : Singleton<PlatformMeshHandler>
    {
        public Platform GeneratePlatform(Vector3 dimensions, Material material, Vector3 position)
        {
            GameObject newPlatformObj = new GameObject("Platform");
            newPlatformObj.transform.position = position;
            Platform platform = newPlatformObj.AddComponent<Platform>();
            platform.Initialize(dimensions, material);
            MeshFilter meshFilter = newPlatformObj.AddComponent<MeshFilter>();
            MeshRenderer meshRenderer = newPlatformObj.AddComponent<MeshRenderer>();
            meshRenderer.material = material;
        
            Mesh mesh = MeshGenerator.CreateMesh(dimensions);
            meshFilter.mesh = mesh;
            return platform;
        }
        
        public Platform SlicePlatform(Platform originalPlatform, float leftBound, float rightBound)
        {
            float sliceLeft = Mathf.Max(leftBound, originalPlatform.transform.position.x);
            float sliceRight = Mathf.Min(rightBound, originalPlatform.transform.position.x + originalPlatform.Dimensions.x);

            if (sliceLeft >= sliceRight)
                return null;

            float newWidth = sliceRight - sliceLeft;
            Vector3 newPosition = new Vector3(sliceLeft, originalPlatform.transform.position.y, originalPlatform.transform.position.z);

            return GeneratePlatform(new Vector3(newWidth, originalPlatform.Dimensions.y, originalPlatform.Dimensions.z), originalPlatform.PlatformMaterial, newPosition);
        }
    }
}