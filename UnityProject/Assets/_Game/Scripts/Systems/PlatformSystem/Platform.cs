using UnityEngine;

namespace _Game.Systems.PlatformSystem
{
    public class Platform : MonoBehaviour
    {
        private MeshFilter _meshFilter;
        private MeshRenderer _meshRenderer;
        private Vector3 _dimensions;
        private Material _platformMaterial;
        private GameObject _slicedPart;
        
        public Vector3 Dimensions => _dimensions;
        public Material PlatformMaterial => _platformMaterial;
    
        public void Initialize(Vector3 dimensions, Material material)
        {
            _dimensions = dimensions;
            _platformMaterial = material;
        }
    }
}