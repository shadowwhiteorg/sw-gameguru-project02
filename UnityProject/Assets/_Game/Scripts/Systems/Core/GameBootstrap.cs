using _Game.Systems.CharacterSystem;
using _Game.Systems.LevelSystem;
using _Game.Systems.MeshSystem;
using _Game.Systems.MovementSystem;
using _Game.Systems.PlatformSystem;

namespace _Game.Systems.Core
{
    using UnityEngine;

    public class GameBootstrap : MonoBehaviour
    {
        [Header("Core Systems")]
        [SerializeField] private MeshHandler meshHandlerPrefab;
        [SerializeField] private PlatformOperator platformOperatorPrefab;
        [SerializeField] private PlatformMovement platformMovementPrefab;
        [SerializeField] private LevelManager levelManagerPrefab;

        private void Awake()
        {
            // Instantiate systems
            var meshHandler = Instantiate(meshHandlerPrefab);
            var levelManager = Instantiate(levelManagerPrefab);
            var platformMovement = Instantiate(platformMovementPrefab);
            var platformOperator = Instantiate(platformOperatorPrefab);
            
            // Get singleton instances
            var playerController = PlayerController.Instance;
            var uiController = UIController.Instance;

            // Initialize dependencies
            meshHandler.Initialize(playerController, platformMovement);
            platformMovement.Initialize(levelManager);
            platformOperator.Initialize(meshHandler, levelManager);
            levelManager.Initialize(platformOperator,playerController,meshHandler);
            uiController.Initialize(levelManager);
        }
    }
}