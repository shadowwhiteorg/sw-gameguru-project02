﻿using System.Collections.Generic;
using _Game.Systems.PlatformSystem;
using _Game.Utils;
using UnityEngine;

namespace _Game.Systems.MovementSystem
{
    public class PlatformMovement : Singleton<PlatformMovement>
    {
        [SerializeField] private List<Platform> activePlatforms = new List<Platform>();
        [SerializeField] private float platformSpeed = 5f;
        [SerializeField] private float removeZThreshold = -10f; // Remove when behind player

        public float PlatformSpeed => platformSpeed;

        void Update()
        {
            MovePlatforms();
            CleanupPlatforms();
        }

        private void MovePlatforms()
        {
            foreach (var platform in activePlatforms)
            {
                platform.transform.position += Vector3.back * (platformSpeed * Time.deltaTime);
            }
        }

        private void CleanupPlatforms()
        {
            for (int i = activePlatforms.Count - 1; i >= 0; i--)
            {
                if (activePlatforms[i].transform.position.z < removeZThreshold)
                {
                    Destroy(activePlatforms[i].gameObject);
                    activePlatforms.RemoveAt(i);
                }
            }
        }

        public void RegisterPlatform(Platform newPlatform)
        {
            activePlatforms.Add(newPlatform);
        }
    }
}