﻿using _Game.DataStructures;
using _Game.Systems.LevelSystem;
using _Game.Utils;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Game.Systems.Core
{
    public class UIController : Singleton<UIController>
    {
        [SerializeField] private Canvas canvas;
        [SerializeField] private GameObject winPanel;
        [SerializeField] private GameObject losePanel;
        [SerializeField] private GameObject startPanel;
        [SerializeField] private GameObject inGamePanel;
        [SerializeField] private Button startButton;
        [SerializeField] private Button nextLevelButton;
        [SerializeField] private Button restartButton;
        [SerializeField] private TextMeshProUGUI levelNumberText;
        
        private LevelManager _levelManager;

        public void Initialize(LevelManager levelManager)
        {
            _levelManager = levelManager;
            OnLevelInitialized();
        }
        
        private void InitializeButtons()
        {
            startButton.onClick.RemoveAllListeners();
            startButton.onClick.AddListener(()=>
            {
                EventBus.Fire(new OnButtonClickedEvent());
                EventBus.Fire(new OnLevelStartEvent());
            });
            
            nextLevelButton.onClick.RemoveAllListeners();
            nextLevelButton.onClick.AddListener(()=>
            {
                EventBus.Fire(new OnButtonClickedEvent());
                EventBus.Fire(new OnLevelInitializeEvent());
            });
            
            restartButton.onClick.RemoveAllListeners();
            restartButton.onClick.AddListener(()=>
            {
                EventBus.Fire(new OnButtonClickedEvent());
                EventBus.Fire(new OnLevelInitializeEvent());
            });
        }

        private void OnLevelInitialized()
        {
            InitializeButtons();
            InitializePanels();
            levelNumberText.text = _levelManager.CurrentLevel.ToString();
        }

        private void OnLevelWin()
        {
            inGamePanel.SetActive(false);
            winPanel.SetActive(true);
        }

        private void OnLevelLose()
        {
            inGamePanel.SetActive(false);
            losePanel.SetActive(true);
        }

        private void InitializePanels()
        {
            inGamePanel.SetActive(false);
            winPanel.SetActive(false);
            losePanel.SetActive(false);
            startPanel.SetActive(true);
        }

        private void OnStartLevel()
        {
            startPanel.SetActive(false);
            inGamePanel.SetActive(true);
        }

        private void OnEnable()
        {
            EventBus.Subscribe<OnLevelInitializeEvent>(e => OnLevelInitialized());
            EventBus.Subscribe<OnLevelStartEvent>(e=> OnStartLevel());
            EventBus.Subscribe<OnLevelWinEvent>(e=> OnLevelWin());
            EventBus.Subscribe<OnLevelFailEvent>(e=> OnLevelLose());
        }

        private void OnDisable()
        {
            EventBus.Unsubscribe<OnLevelInitializeEvent>(e=>OnLevelInitialized());
            EventBus.Unsubscribe<OnLevelStartEvent>(e=> OnStartLevel());
            EventBus.Unsubscribe<OnLevelWinEvent>(e=> OnLevelWin());
            EventBus.Unsubscribe<OnLevelFailEvent>(e=> OnLevelLose());
        }
    }
}