using System;
using _Game.Systems.MeshSystem;
using _Game.Systems.Test;
using UnityEngine;

namespace _Game.Systems.Core
{
    public class InputHandler : MonoBehaviour
    {
        [SerializeField] private PlatformMeshHandlerTest meshHandlerTest;
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.G))
            {
                meshHandlerTest.GenerateTestPlatform();
            }

            if (Input.GetKeyDown(KeyCode.S))
            {
                meshHandlerTest.SliceTestPlatform();
            }
        }
    }
}