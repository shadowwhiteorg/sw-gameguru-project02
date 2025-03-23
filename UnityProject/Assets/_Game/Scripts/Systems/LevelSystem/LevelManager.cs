using System;
using System.Collections;
using _Game.DataStructures;
using _Game.Systems.CharacterSystem;
using _Game.Systems.MeshSystem;
using _Game.Systems.PlatformSystem;
using _Game.Utils;
using UnityEngine;

namespace _Game.Systems.LevelSystem
{
    public class LevelManager : Singleton<LevelManager>
    {
        [SerializeField] private LevelDataCatalog levelDataCatalog;
        [SerializeField] private GameObject finalPlatformPrefab;

        private int _currentStep;
        private bool _isFinalStep;
        private Vector3 _finalPosition;
        private GameObject _finalPlatform;
        private PlayerController _player;
        public int CurrentLevel => PlayerPrefs.GetInt(GameConstants.PlayerPrefsLevel, 1);
        public LevelData CurrentLevelData => levelDataCatalog.Levels[CurrentLevel % levelDataCatalog.Levels.Count];
        

        private void IncreaseStep()
        {
            _currentStep++;
            if (_currentStep >= CurrentLevelData.NumberOfPlatforms)
            {
                PlatformOperator.Instance.SetCanCreatePlatform(false);
                _isFinalStep = true;
                StartCoroutine(CheckFinalLine());
            }
        }

        private void Initialize()
        {
            _player = PlayerController.Instance;
            _finalPosition = (CurrentLevelData.NumberOfPlatforms*MeshHandler.Instance.PlatformLength + GameConstants.FirstPlatformOffset )*Vector3.forward;
            _finalPlatform = Instantiate(finalPlatformPrefab, _finalPosition, Quaternion.identity);
            _currentStep = 1;
            _isFinalStep = false;
        }

        private IEnumerator CheckFinalLine()
        {
            yield return new WaitUntil(() => _finalPlatform.transform.position.z <= _player.transform.position.z-GameConstants.CrossPlatformDistance);
            EventBus.Fire(new OnLevelWinEvent());
        }
        

        private void OnEnable()
        {
            EventBus.Subscribe<OnLevelInitializeEvent>(e=>Initialize());
            EventBus.Subscribe<OnStopPlatformEvent>(e=> IncreaseStep());
        }

        private void OnDisable()
        {
            EventBus.Unsubscribe<OnLevelInitializeEvent>(e=>Initialize());
            EventBus.Unsubscribe<OnStopPlatformEvent>(e=> IncreaseStep());

        }
    }
}