using _Game.DataStructures;
using _Game.Utils;
using UnityEngine;

namespace _Game.Systems.Core
{
    public class InputHandler : MonoBehaviour
    {
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                EventBus.Fire(new OnStopPlatformEvent());
            }

            if (Input.GetKeyDown(KeyCode.S))
            {
                EventBus.Fire(new OnLevelStartEvent());
            }

            if (Input.GetKeyDown(KeyCode.W))
            {
                EventBus.Fire(new OnLevelInitializeEvent());
            }
        }
    }
}