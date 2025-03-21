using _Game.DataStructures;
using _Game.Systems.MeshSystem;
using _Game.Systems.PlatformSystem;
using UnityEngine;
using _Game.Utils;

namespace _Game.Systems.Core
{
    public class GameController : Singleton<GameController>
    {
        private Platform _currentPlatform;
        private Platform _movingPlatform;

        private void OnStopPlatform()
        {
            _movingPlatform.StopMoving();
            if(!SliceCurrentPlatform()) return;
            _movingPlatform.StartFalling();
            _currentPlatform = _movingPlatform;
            CreateNewMovingPlatform();
        }

        private void OnInitializeLevel()
        {
            CreateNewPlatform();
            CreateNewMovingPlatform();
        }

        private void CreateNewPlatform()
        {
            _currentPlatform = MeshHandler.Instance.GeneratePlatform( _currentPlatform ? _currentPlatform.MainPartPivot : Vector3.zero);
        }
        
        private void CreateNewMovingPlatform()
        {
            Vector3 newPosition = _currentPlatform.MainPartPivot + new Vector3(0, 0,_currentPlatform.MainPartSize.z);
            _movingPlatform = MeshHandler.Instance.GeneratePlatform( newPosition, _currentPlatform.MainPartSize.x);
            _movingPlatform.MoveMainPart();
        }

        private bool SliceCurrentPlatform()
        {
            MeshHandler.Instance.SlicePlatform(_movingPlatform, _currentPlatform.MainPartPivot.x,
                _currentPlatform.MainPartPivot.x + _currentPlatform.MainPartSize.x, out var successful);
            return successful;
        }
        
        private void OnEnable()
        {
            EventBus.Subscribe<OnLevelInitializeEvent>(e => OnInitializeLevel());
            EventBus.Subscribe<OnStopPlatformEvent>(e => OnStopPlatform());
        }

        private void OnDisable()
        {
            EventBus.Unsubscribe<OnLevelInitializeEvent>(e => OnInitializeLevel());
            EventBus.Unsubscribe<OnStopPlatformEvent>(e => OnStopPlatform());
        }
        
    }
}