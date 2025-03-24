using System;
using _Game.DataStructures;
using _Game.Scripts.Enums;
using _Game.Utils;
using UnityEngine;

namespace _Game.Systems.Core
{
    public class InputHandler : MonoBehaviour
    {
        private bool _canStopPlatform;
        
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
            {
                if(GameManager.Instance.GameState == GameState.InGame && _canStopPlatform)
                    EventBus.Fire(new OnStopPlatformEvent());
            }
        }

        private void OnEnable()
        {
            EventBus.Subscribe<OnStopPlatformEvent>(e=> _canStopPlatform = false);
            EventBus.Subscribe<OnPlayerChangedPlatformEvent>(e=> _canStopPlatform = true);
        }
        
        private void OnDisable()
        {
            EventBus.Unsubscribe<OnStopPlatformEvent>(e=> _canStopPlatform = false);
            EventBus.Unsubscribe<OnPlayerChangedPlatformEvent>(e=> _canStopPlatform = true);
        }
    }
}