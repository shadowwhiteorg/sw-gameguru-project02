using System;
using _Game.DataStructures;
using _Game.Scripts.Enums;
using _Game.Utils;
using UnityEngine;

namespace _Game.Systems.Core
{
    public class GameManager : Singleton<GameManager>
    {
        private GameState _gameState;
        public GameState GameState =>_gameState;

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
    }
}