using _Game.DataStructures;
using _Game.Scripts.Enums;
using _Game.Utils;

namespace _Game.Systems.Core
{
    public class GameManager : Singleton<GameManager>
    {
        private GameState _gameState;
        public GameState GameState =>_gameState;


        private void Start()
        {
            EventBus.Fire(new OnLevelInitializeEvent());
        }

        public void SetGameState(GameState gameState)
        {
            _gameState = gameState;
        }

        private void OnEnable()
        {
            EventBus.Subscribe<OnLevelInitializeEvent>(e=> SetGameState(GameState.Start));
            EventBus.Subscribe<OnLevelStartEvent>(e=> SetGameState(GameState.InGame));
            EventBus.Subscribe<OnLevelFailEvent>(e=> SetGameState(GameState.LevelEnd));
            EventBus.Subscribe<OnLevelWinEvent>(e=> SetGameState(GameState.LevelEnd));
        }
        
        private void OnDisable()
        {
            EventBus.Unsubscribe<OnLevelInitializeEvent>(e=> SetGameState(GameState.Start));
            EventBus.Unsubscribe<OnLevelStartEvent>(e=> SetGameState(GameState.InGame));
            EventBus.Unsubscribe<OnLevelFailEvent>(e=> SetGameState(GameState.LevelEnd));
            EventBus.Unsubscribe<OnLevelWinEvent>(e=> SetGameState(GameState.LevelEnd));
        }
    }
}