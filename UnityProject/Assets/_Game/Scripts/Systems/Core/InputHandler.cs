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
            if (Input.GetKeyDown(KeyCode.Space))
            {
                // meshHandlerTest.TestGenerationAndSliceButton();
            }
        }
    }
}