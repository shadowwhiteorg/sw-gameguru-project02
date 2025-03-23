using UnityEngine;

namespace _Game.Systems.LevelSystem
{
    [CreateAssetMenu(fileName = "LevelData", menuName = "FallingGrounds/LevelData", order = 0)]
    public class LevelData : ScriptableObject
    {
        public int NumberOfPlatforms;
        public float PlatformSpeed;
        // Further implementation
        public float InitialLevelSpeed;
        public float FinalLevelSpeed;
        
    }
}