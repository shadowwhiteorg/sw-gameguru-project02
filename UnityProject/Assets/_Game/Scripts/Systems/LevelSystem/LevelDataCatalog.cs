using System.Collections.Generic;
using UnityEngine;

namespace _Game.Systems.LevelSystem
{
    [CreateAssetMenu(fileName = "LevelDataCatalog", menuName = "FallingGrounds/LevelDataCatalog", order = 0)]
    public class LevelDataCatalog : ScriptableObject
    {
        public List<LevelData> Levels;
    }
}