using UnityEngine;

namespace _Game.Systems.Core
{
    public class Platform : MonoBehaviour
    {
        public Vector3 Dimensions { get; private set; }
        public Material PlatformMaterial { get; private set; }
        private MeshFilter _meshFilter;
        private MeshRenderer _meshRenderer;
    
        public void Initialize(Vector3 dimensions, Material material)
        {
            Dimensions = dimensions;
            PlatformMaterial = material;
        }
    }
}