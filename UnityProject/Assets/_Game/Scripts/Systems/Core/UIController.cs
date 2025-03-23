using _Game.DataStructures;
using _Game.Utils;
using UnityEngine;
using UnityEngine.UI;

namespace _Game.Systems.Core
{
    public class UIController : MonoBehaviour
    {
        [SerializeField] private Canvas canvas;
        [SerializeField] private GameObject winPanel;
        [SerializeField] private GameObject losePanel;
        [SerializeField] private GameObject startPanel;
        [SerializeField] private GameObject inGamePanel;
        [SerializeField] private Button startButton;
        [SerializeField] private Button nextLevelButton;
        [SerializeField] private Button restartButton;
        
        
        private void InitializeButtons()
        {
            startButton.onClick.RemoveAllListeners();
            startButton.onClick.AddListener(()=> EventBus.Fire(new OnLevelStartEvent()));
            
            nextLevelButton.onClick.RemoveAllListeners();
            nextLevelButton.onClick.AddListener(()=> EventBus.Fire(new OnLoadLevelEvent()));
            
            restartButton.onClick.RemoveAllListeners();
            restartButton.onClick.AddListener(()=> EventBus.Fire(new OnLoadLevelEvent()));
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

        private void OnLoadLevel()
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
            EventBus.Subscribe<OnLevelInitializeEvent>(e=>InitializeButtons());
            EventBus.Subscribe<OnLevelStartEvent>(e=> OnStartLevel());
            EventBus.Subscribe<OnLevelWinEvent>(e=> OnLevelWin());
            EventBus.Subscribe<OnLevelFailEvent>(e=> OnLevelLose());
            EventBus.Subscribe<OnLoadLevelEvent>(e=> OnLoadLevel());
        }

        private void OnDisable()
        {
            EventBus.Unsubscribe<OnLevelInitializeEvent>(e=>InitializeButtons());
            EventBus.Unsubscribe<OnLevelStartEvent>(e=> OnStartLevel());
            EventBus.Unsubscribe<OnLevelWinEvent>(e=> OnLevelWin());
            EventBus.Unsubscribe<OnLevelFailEvent>(e=> OnLevelLose());
            EventBus.Unsubscribe<OnLoadLevelEvent>(e=> OnLoadLevel());
        }
    }
}