using UnityEngine;

namespace _Game.Systems.PlatformSystem
{
    public class Platform : MonoBehaviour
    {
        private Vector3 _dimensions;
        private Material _platformMaterial;
        private GameObject _mainPart;
        private GameObject _slicedPart;
        private bool _isMoving;
        private float _speed = 3f;
        private float _range = 5f;
        private Vector3 _startPosition;

        public GameObject MainPart => _mainPart;
        public Vector3 Dimensions => _dimensions;
        public Material PlatformMaterial => _platformMaterial;
        public Vector3 MainPartCenterPosition => _mainPart.GetComponent<MeshRenderer>().bounds.center;
        public Vector3 MainPartSize => _mainPart.GetComponent<MeshRenderer>().bounds.size;

        public Vector3 MainPartPivot => new Vector3(MainPartCenterPosition.x - MainPartSize.x / 2,
            MainPartCenterPosition.y - MainPartSize.y / 2, MainPartCenterPosition.z - MainPartSize.z / 2);
        
        public void Initialize(GameObject mainPart)
        {
            // _dimensions = dimensions;
            // _platformMaterial = material;
            _mainPart = mainPart;
            _isMoving = false;
        }
        
        public void SetSlicedPart(GameObject slicedPart)
        {
            _slicedPart = slicedPart;
        }
        
        public void SetMainPart(GameObject mainPart)
        {
            _mainPart = mainPart;
        }
        
        public void StartMoving()
        {
            _isMoving = true;
            _startPosition = transform.position - Vector3.right * _range;
        }

        public void StopMoving()
        {
            _isMoving = false;
        }
        
        private void Update()
        {
            if (_isMoving)
            {
                float x = _startPosition.x + Mathf.PingPong(Time.time * _speed, _range*2);
                transform.position = new Vector3(x, transform.position.y, transform.position.z);
            }
        }
    }
}