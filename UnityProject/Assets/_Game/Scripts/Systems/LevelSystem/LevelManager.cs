using System;
using System.Collections;
using System.Collections.Generic;
using _Game.DataStructures;
using _Game.Interfaces;
using _Game.Scripts.Enums;
using _Game.Systems.CharacterSystem;
using _Game.Systems.Core;
using _Game.Systems.MeshSystem;
using _Game.Systems.MovementSystem;
using _Game.Systems.PlatformSystem;
using _Game.Utils;
using UnityEngine;

namespace _Game.Systems.LevelSystem
{
    public class LevelManager : Singleton<LevelManager>,ILevelManager
    {
        [SerializeField] private LevelDataCatalog levelDataCatalog;
        [SerializeField] private GameObject finalPlatformPrefab;

        private int _currentStep;
        private int _currentLevel;
        private Vector3 _finalPosition;
        private PlatformOperator _platformOperator;
        private PlatformMovement _platformMovement;
        private MeshHandler _meshHandler;
        private GameObject _finalPlatform;
        private PlayerController _player;
        private List<GameObject> _levelObjects = new List<GameObject>();
        public int CurrentLevel => PlayerPrefs.GetInt(GameConstants.PlayerPrefsLevel, 1);
        public LevelData CurrentLevelData => levelDataCatalog.Levels[CurrentLevel % levelDataCatalog.Levels.Count];
        
        public void Initialize(PlatformOperator @operator, MeshHandler meshHandler, PlayerController player, PlatformMovement platformMovement)
        {
            _platformOperator = @operator;
            _meshHandler = meshHandler;
            _player = player;
            _platformMovement = platformMovement;
        }

        private void IncreaseStep()
        {
            _currentStep++;
            if (_currentStep >= CurrentLevelData.NumberOfPlatforms)
            {
                _platformOperator.SetCanCreatePlatform(false);
                GameManager.Instance.SetGameState(GameState.LevelEnd);
                StartCoroutine(CheckFinalLine());
            }
        }

        private void UpdateCurrentLevel()
        {
            _currentLevel = CurrentLevel + 1;
            PlayerPrefs.SetInt(GameConstants.PlayerPrefsLevel, _currentLevel);
        }

        private void Initialize()
        {
            ResetLevelObjects();
            _levelObjects.Add(Instantiate(finalPlatformPrefab, Vector3.zero, Quaternion.identity));
            _finalPosition = (CurrentLevelData.NumberOfPlatforms*_meshHandler.PlatformLength + GameConstants.FirstPlatformOffset )*Vector3.forward;
            _finalPlatform = Instantiate(finalPlatformPrefab, _finalPosition, Quaternion.identity);
            _finalPlatform.GetComponent<ParallaxObject>().Initialize(_platformMovement,this);
            _levelObjects.Add(_finalPlatform);
            _currentStep = 1;
            RegisterEvents();
        }

        private IEnumerator CheckFinalLine()
        {
            yield return new WaitUntil(() => _finalPlatform.transform.position.z <= _player.transform.position.z-GameConstants.FinalPlatformDistance);
            EventBus.Fire(new OnLevelWinEvent());
        }

        public void RegisterLevelObject(GameObject levelObject)
        {
            _levelObjects.Add(levelObject);
        }
        
        private void ResetLevelObjects()
        {
            foreach (var levelObject in _levelObjects)
            {
                Destroy(levelObject);
            }
            _levelObjects.Clear();
        }

        private void RegisterEvents()
        {
            EventBus.Subscribe<OnLevelInitializeEvent>(e=>Initialize());
            EventBus.Subscribe<OnStopPlatformEvent>(e=> IncreaseStep());
            EventBus.Subscribe<OnLevelWinEvent>(e=>UpdateCurrentLevel());
        }
        private void OnEnable()
        {
            
        }

        private void OnDisable()
        {
            EventBus.Unsubscribe<OnLevelInitializeEvent>(e=>Initialize());
            EventBus.Unsubscribe<OnStopPlatformEvent>(e=> IncreaseStep());
            EventBus.Unsubscribe<OnLevelWinEvent>(e=>UpdateCurrentLevel());
        }
    }
}