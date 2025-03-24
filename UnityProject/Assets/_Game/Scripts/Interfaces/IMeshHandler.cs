using _Game.Systems.PlatformSystem;
using UnityEngine;

namespace _Game.Interfaces
{
    public interface IMeshHandler
    {
        Platform GeneratePlatform(Vector3 position, float platformWidth = 0);
        void SlicePlatform(Platform platform, float leftBound, float rightBound, out bool successful);
        float PlatformLength { get; }
    }
}