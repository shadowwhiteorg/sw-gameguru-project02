using _Game.Systems.MeshSystem;
using UnityEngine;

namespace _Game.Systems.Core
{
    public class GameController : MonoBehaviour
    {
        [SerializeField] private PlatformMeshHandler _meshHandler;
        private Transform _currentPlatform;
        public Transform CurrentPlatform => _currentPlatform;

    }
}