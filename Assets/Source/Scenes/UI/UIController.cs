using System;
using Helicopter.Core.Managers.Audio.Signals;
using Helicopter.Core.Scenes.UI.Factories;
using Helicopter.Core.Scenes.UI.Popups;
using Helicopter.Core.Services.Game.Signals;
using Zenject;

namespace Helicopter.Core.Scenes.UI
{
    public class UIController : IInitializable, IDisposable
    {
        private readonly UIView _view;

        [Inject]
        private IUIFactory _uiFactory;
        
        [Inject]
        private SignalBus _signalBus;

        public UIController(UIView view, float musicVolume, float soundVolume)
        {
            _view = view;
            _view.SetStartingValues(musicVolume, soundVolume);
        }

        public void Initialize()
        {
            _view.InfoButtonClicked += ViewOnInfoButtonClicked;
            _view.ResetButtonClicked += ViewOnResetButtonClicked;
            _view.MusicValueChanged += ViewOnMusicValueChanged;
            _view.SoundValueChanged += ViewOnSoundValueChanged;
        }
        
        public void Dispose()
        {
            _view.InfoButtonClicked -= ViewOnInfoButtonClicked;
            _view.ResetButtonClicked -= ViewOnResetButtonClicked;
            _view.MusicValueChanged -= ViewOnMusicValueChanged;
            _view.SoundValueChanged -= ViewOnSoundValueChanged;
        }

        private async void ViewOnInfoButtonClicked()
        {
            var popup = await _uiFactory.CreateInfoPopup(_view.transform);
            popup.Closed += OnPopupClosed;
            return;

            void OnPopupClosed(InfoPopupView infoPopupView)
            {
                infoPopupView.Closed -= OnPopupClosed;
                _uiFactory.ReturnInfoPopup(infoPopupView);
            }
        }

        private void ViewOnResetButtonClicked()
        {
            _signalBus.Fire<ResetHelicopterRequestSignal>();
        }

        private void ViewOnMusicValueChanged(float value)
        {
            var signal = new SetMusicVolumeSignal(value);
            _signalBus.Fire(signal);
        }

        private void ViewOnSoundValueChanged(float value)
        {
            var signal = new SetSoundVolumeSignal(value);
            _signalBus.Fire(signal);
        }
    }
}