using _Game.Systems.LevelSystem;
using UnityEngine;

namespace _Game.Interfaces
{
    public interface ILevelManager
    {
        void RegisterLevelObject(GameObject levelObject);
        int CurrentLevel { get; }
        LevelData CurrentLevelData { get; }
    }
}