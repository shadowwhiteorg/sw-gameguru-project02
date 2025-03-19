using _Game.Systems.Core;
using _Game.Systems.MeshSystem;
using UnityEngine;

namespace _Game.Systems.Test
{
    public class PlatformMeshHandlerTest : MonoBehaviour
    {
        public PlatformMeshHandler meshHandler;
        public Material testMaterial; // Assign a material in the inspector

        public Vector3 platformDimensions = new Vector3(5, 1, 2);
        public Vector3 platformPosition = Vector3.zero;

        public float sliceLeftBound = 1f;
        public float sliceRightBound = 4f;
        public float sliceZ;

        public void GenerateAndSlicePlatform()
        {
            if (meshHandler == null || testMaterial == null)
            {
                Debug.LogError("MeshHandler or testMaterial is not assigned!");
                return;
            }

            // Generate a platform
            // Platform generatedPlatform = meshHandler.GeneratePlatform(platformDimensions, testMaterial, platformPosition);
            // meshHandler.SlicePlatform(generatedPlatform, sliceZ);
            // if (generatedPlatform != null)
            // {
            //     Debug.Log($"Generated platform at {generatedPlatform.transform.position} with dimensions {generatedPlatform.Dimensions}");
            //
            //     // Slice the platform
            //     // meshHandler.SlicePlatform(generatedPlatform, sliceZ);
            //     // Platform[] slicedPlatforms = meshHandler.SlicePlatform(generatedPlatform, sliceLeftBound, sliceRightBound);
            //
            //     // if (slicedPlatforms.Length != 0)
            //     // {
            //     //     // Debug.Log($"Sliced platform at {slicedPlatform.transform.position} with dimensions {slicedPlatform.Dimensions}");
            //     // }
            // }
        }
    }
}