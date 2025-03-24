using _Game.DataStructures;
using _Game.Systems.CharacterSystem;
using _Game.Systems.LevelSystem;
using _Game.Systems.MeshSystem;
using _Game.Systems.MovementSystem;
using _Game.Systems.PlatformSystem;
using _Game.Utils;
using UnityEngine;

namespace _Game.Systems.Core
{
    public class GameBootstrap : MonoBehaviour
    {
        [Header("Core Systems")]
        [SerializeField] private MeshHandler _meshHandlerPrefab;
        [SerializeField] private PlatformOperator _platformOperatorPrefab;
        [SerializeField] private PlatformMovement _platformMovementPrefab;
        [SerializeField] private LevelManager _levelManagerPrefab;
        

        private void Awake()
        {
            // Instantiate systems
            var meshHandler = Instantiate(_meshHandlerPrefab);
            var platformMovement = Instantiate(_platformMovementPrefab);
            var platformOperator = Instantiate(_platformOperatorPrefab);
            var levelManager = LevelManager.Instance;
            var playerController = PlayerController.Instance;
            var uiController = UIController.Instance;

            // Initialize dependencies
            platformMovement.Initialize(levelManager);
            platformOperator.Initialize(
                meshHandler,
                levelManager,
                platformMovement,
                playerController
            );
            levelManager.Initialize(platformOperator, meshHandler,playerController,platformMovement);
            meshHandler.Initialize(playerController);
            uiController.Initialize(levelManager);


            // Start the game
            EventBus.Fire(new OnLevelInitializeEvent());
        }
    }
}