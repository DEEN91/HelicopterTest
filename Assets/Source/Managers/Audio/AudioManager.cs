using System;
using Helicopter.Core.Managers.Audio.Signals;
using UnityEngine;
using Zenject;

namespace Helicopter.Core.Managers.Audio
{
    public class AudioManager : MonoBehaviour
    {
        [SerializeField] 
        public AudioSource _musicSource;
        [SerializeField]
        public AudioSource _topRotorSource;
        [SerializeField] 
        public AudioSource _tailRotorSource;
        
        [Inject]
        private SignalBus _signalBus;

        private void OnEnable()
        {
            _signalBus.Subscribe<SetMusicVolumeSignal>(SetMusicVolumeSignalHandler);
            _signalBus.Subscribe<SetSoundVolumeSignal>(SetSoundVolumeSignalHandler);
        }

        private void OnDisable()
        {
            _signalBus.Unsubscribe<SetMusicVolumeSignal>(SetMusicVolumeSignalHandler);
            _signalBus.Unsubscribe<SetSoundVolumeSignal>(SetSoundVolumeSignalHandler);
        }

        private void SetMusicVolumeSignalHandler(SetMusicVolumeSignal signal)
        {
            _musicSource.volume = signal.Value;
        }

        private void SetSoundVolumeSignalHandler(SetSoundVolumeSignal signal)
        {
            _tailRotorSource.volume = signal.Value*0.8f;
            _topRotorSource.volume = signal.Value;
        }
    }
}