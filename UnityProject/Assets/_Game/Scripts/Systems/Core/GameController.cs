﻿using _Game.Systems.MeshSystem;
using _Game.Systems.MovementSystem;
using _Game.Systems.PlatformSystem;
using UnityEngine;
using UnityEngine.Serialization;

namespace _Game.Systems.Core
{
    public class GameController : MonoBehaviour
    {
        [SerializeField] private PlatformMeshHandler meshHandler;
        [SerializeField]private Platform _currentPlatform;
        [SerializeField] private Platform _movingPlatform;
        private void Start()
        {
           CreateNewPlatform();
           CreateNewMovingPlatform();
        }
        
        private void Update()
        {
            
            if (Input.GetKeyDown(KeyCode.P))
            {
                _movingPlatform.StopMoving();
                if(!SliceCurrentPlatform()) return;
                _movingPlatform.StartFalling();
                _currentPlatform = _movingPlatform;
                CreateNewMovingPlatform();
            }

            if (Input.GetKeyDown(KeyCode.O))
                PlatformMovement.Instance.StartMovement();
        }

        private void CreateNewPlatform()
        {
            _currentPlatform = meshHandler.GeneratePlatform( _currentPlatform ? _currentPlatform.MainPartPivot : Vector3.zero);
        }
        
        private void CreateNewMovingPlatform()
        {
            Vector3 newPosition = _currentPlatform.MainPartPivot + new Vector3(0, 0,_currentPlatform.MainPartSize.z);
            _movingPlatform = meshHandler.GeneratePlatform( newPosition, _currentPlatform.MainPartSize.x);
            _movingPlatform.MoveMainPart();
            // _movingPlatform.StartMoving();
        }
        
        private bool SliceCurrentPlatform()
        {
            meshHandler.SlicePlatform(_movingPlatform, _currentPlatform.MainPartPivot.x, _currentPlatform.MainPartPivot.x + _currentPlatform.MainPartSize.x, out var successful);
            return successful;
        }
    }
}