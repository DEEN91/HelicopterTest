using System;
using Helicopter.Core.Utils;
using UnityEngine;
using UnityEngine.UI;

namespace Helicopter.Core.Scenes.UI
{
    public class UIView : MonoBehaviour
    {
        public event Action InfoButtonClicked;
        public event Action ResetButtonClicked;
        public event Action<float> MusicValueChanged;
        public event Action<float> SoundValueChanged;
        
        [SerializeField]
        private Button _infoButton;
        [SerializeField]
        private ButtonWithSlider _musicButton;
        [SerializeField]
        private ButtonWithSlider _soundButton;
        [SerializeField]
        private Button _resetButton;

        public void SetStartingValues(float musicValue, float soundValue)
        {
            _musicButton.SetSliderValue(musicValue);
            _soundButton.SetSliderValue(soundValue);
        }
        
        private void OnEnable()
        {
            _infoButton.onClick.AddListener(OnInfoButtonClicked);
            _musicButton.onClick.AddListener(OnMusicButtonClicked);
            _musicButton.onSliderValueChanged.AddListener(OnMusicSliderChanged);
            _soundButton.onClick.AddListener(OnSoundButtonClicked);
            _soundButton.onSliderValueChanged.AddListener(OnSoundSliderChanged);
            _resetButton.onClick.AddListener(OnResetButtonClicked);
        }

        private void OnDisable()
        {
            _infoButton.onClick.RemoveListener(OnInfoButtonClicked);
            _musicButton.onClick.RemoveListener(OnMusicButtonClicked);
            _musicButton.onSliderValueChanged.RemoveListener(OnMusicSliderChanged);
            _soundButton.onClick.RemoveListener(OnSoundButtonClicked);
            _soundButton.onSliderValueChanged.RemoveListener(OnSoundSliderChanged);
            _resetButton.onClick.RemoveListener(OnResetButtonClicked);
        }
        
        private void OnInfoButtonClicked()
        {
            InfoButtonClicked?.Invoke();
        }

        private void OnMusicButtonClicked()
        {
            _musicButton.SetSliderActive(!_musicButton.SliderActive);
        }

        private void OnMusicSliderChanged(float value)
        {
            MusicValueChanged?.Invoke(value);
        }

        private void OnSoundButtonClicked()
        {
            _soundButton.SetSliderActive(!_soundButton.SliderActive);
        }
        
        private void OnSoundSliderChanged(float value)
        {
            SoundValueChanged?.Invoke(value);
        }

        private void OnResetButtonClicked()
        {
            ResetButtonClicked?.Invoke();
        }

    }
}