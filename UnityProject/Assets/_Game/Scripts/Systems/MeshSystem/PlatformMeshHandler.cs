using _Game.Systems.PlatformSystem;
using _Game.Utils;
using UnityEngine;

namespace _Game.Systems.MeshSystem
{
    public class PlatformMeshHandler : Singleton<PlatformMeshHandler>
    {
        public Platform GeneratePlatform(Vector3 dimensions, Material material, Vector3 position)
        {
            GameObject mNewPlatformObj = new GameObject("Platform");
            mNewPlatformObj.transform.position = position;
            Platform mPlatform = mNewPlatformObj.AddComponent<Platform>();
            
           GameObject mMainMeshObject = GeneratePlatformMesh(mPlatform, dimensions, material,position,true);
            
            mPlatform.Initialize(dimensions, material,mMainMeshObject);
            
            return mPlatform;
        }

        private GameObject GeneratePlatformMesh(Platform platform, Vector3 dimensions,Material material, Vector3 position, bool isMain)
        {
            GameObject mMainMeshObject = new GameObject( isMain ? "MainMesh":"SlicedMesh");
            mMainMeshObject.transform.position = position;
            mMainMeshObject.transform.SetParent(platform.transform);
            
            MeshFilter mMeshFilter = mMainMeshObject.AddComponent<MeshFilter>();
            MeshRenderer mMeshRenderer = mMainMeshObject.AddComponent<MeshRenderer>();
            mMeshRenderer.material = material;
        
            Mesh mMesh = MeshGenerator.CreateMesh(dimensions);
            mMeshFilter.mesh = mMesh;
            return mMeshFilter.gameObject;
        }

        public void SlicePlatform(Platform platform, float sliceZ, bool leftSlide)
        {
            float sliceLeft = Mathf.Max(sliceZ, platform.transform.position.x);
            float sliceRight = Mathf.Min(sliceZ, platform.transform.position.x + platform.Dimensions.x);
            
            float newWidthLeft = sliceLeft - platform.transform.position.x;
            float newWidthRight = (platform.transform.position.x + platform.Dimensions.x) - sliceRight;

            Vector3 newPositionLeft = new Vector3(platform.transform.position.x, platform.transform.position.y, platform.transform.position.z);
            Vector3 newPositionRight = new Vector3(sliceRight, platform.transform.position.y, platform.transform.position.z);
            
            platform.MainPart?.SetActive(false);
            GeneratePlatformMesh(platform,new Vector3(newWidthLeft, platform.Dimensions.y, platform.Dimensions.z), platform.PlatformMaterial, newPositionLeft,false);
            GeneratePlatformMesh(platform,new Vector3(newWidthRight, platform.Dimensions.y, platform.Dimensions.z), platform.PlatformMaterial, newPositionRight,true);
            

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