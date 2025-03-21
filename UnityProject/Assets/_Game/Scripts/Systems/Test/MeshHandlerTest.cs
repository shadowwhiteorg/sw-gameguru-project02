using _Game.Systems.Core;
using _Game.Systems.MeshSystem;
using _Game.Systems.PlatformSystem;
using UnityEngine;

namespace _Game.Systems.Test
{
    public class MeshHandlerTest : MonoBehaviour
    {
        public MeshHandler meshHandler;
        public Material testMaterial;

        public Vector3 platformDimensions = new Vector3(5, 1, 2);
        public Vector3 platformPosition = Vector3.zero;

        public float sliceLeftBound = 1f;
        public float sliceRightBound = 4f;
        public float sliceZ;

        private Platform _generatedPlatform;

        public void GenerateTestPlatform()
        {
            // _generatedPlatform = meshHandler.GeneratePlatform(platformDimensions, testMaterial, platformPosition);
        }

        public void SliceTestPlatform()
        {
            // meshHandler.SlicePlatform(_generatedPlatform, sliceZ,true);
        }
    }
}