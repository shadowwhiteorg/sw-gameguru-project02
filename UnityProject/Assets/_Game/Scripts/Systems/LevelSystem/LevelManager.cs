using _Game.DataStructures;
using UnityEngine;

namespace _Game.Systems.LevelSystem
{
    public class LevelManager : MonoBehaviour
    {
        
        
        [SerializeField] private LevelDataCatalog levelDataCatalog;

        public int CurrentLevel => PlayerPrefs.GetInt(GameConstants.PlayerPrefsLevel, 1);
        public LevelData CurrentLevelData => levelDataCatalog.Levels[CurrentLevel % levelDataCatalog.Levels.Count];
        
        
        
        
    }
}