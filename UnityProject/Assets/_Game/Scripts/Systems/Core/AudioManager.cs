using _Game.DataStructures;
using _Game.Utils;
using UnityEngine;

namespace _Game.Systems.Core
{
    public class AudioManager : MonoBehaviour
    {
        [SerializeField] private float unitPitchIncrease = 0.1f;
        [SerializeField] private AudioSource platformAudioSource;
        [SerializeField] private AudioSource gameAudioSource;
        [SerializeField] private AudioClip platformPlacementSound;
        [SerializeField] private AudioClip failSound;
        [SerializeField] private AudioClip winSound;
        [SerializeField] private AudioClip clickSound;

        private void PlayPlatformPlacementSound(int comboCount)
        {
            platformAudioSource.pitch = 1 + comboCount * unitPitchIncrease;
            platformAudioSource.clip = platformPlacementSound;
            platformAudioSource.Play();
        }

        private void PlayFailSound()
        {
            gameAudioSource.clip = failSound;
            gameAudioSource.Play();
        }

        private void PlayWinSound()
        {
            gameAudioSource.clip = winSound;
            gameAudioSource.Play();
        }
        
        private void PlayClickSound()
        {
            gameAudioSource.clip = clickSound;
            gameAudioSource.Play();
        }

        private void OnEnable()
        {
            EventBus.Subscribe<OnLevelWinEvent>(e => PlayWinSound());
            EventBus.Subscribe<OnLevelFailEvent>(e => PlayFailSound());
            EventBus.Subscribe<OnButtonClickedEvent>(e => PlayClickSound());
            EventBus.Subscribe<OnComboEvent>(e => PlayPlatformPlacementSound(e.ComboCount));
        }
        
        private void OnDisable()
        {
            EventBus.Unsubscribe<OnLevelWinEvent>(e => PlayWinSound());
            EventBus.Unsubscribe<OnLevelFailEvent>(e => PlayFailSound());
            EventBus.Unsubscribe<OnButtonClickedEvent>(e => PlayClickSound());
            EventBus.Unsubscribe<OnComboEvent>(e => PlayPlatformPlacementSound(e.ComboCount));
        }
    }
}