using UnityEngine;

namespace _Game.Systems.PlatformSystem
{
    public class Platform : MonoBehaviour
    {
        private MeshFilter _meshFilter;
        private MeshRenderer _meshRenderer;
        private Vector3 _dimensions;
        private Material _platformMaterial;
        private GameObject _mainPart;
        private GameObject _slicedPart;

        public GameObject MainPart => _mainPart;
        public Vector3 Dimensions => _dimensions;
        public Material PlatformMaterial => _platformMaterial;
    
        public void Initialize(Vector3 dimensions, Material material, GameObject mainPart)
        {
            _dimensions = dimensions;
            _platformMaterial = material;
            _mainPart = mainPart;
        }

        private void SetSlicedPart(GameObject slicedPart)
        {
            _slicedPart = slicedPart;
        }
        
    }
}