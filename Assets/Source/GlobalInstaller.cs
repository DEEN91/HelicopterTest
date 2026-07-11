using Helicopter.Core.Managers.Audio;
using Helicopter.Core.Managers.Audio.Signals;
using Helicopter.Core.Scenes.Helicopter;
using Helicopter.Core.Scenes.Helicopter.HUD;
using Helicopter.Core.Scenes.UI;
using Helicopter.Core.Scenes.UI.Factories;
using Helicopter.Core.Scenes.UI.Popups;
using Helicopter.Core.Services.Game;
using Helicopter.Core.Services.Game.Signals;
using Helicopter.Core.Services.PlayerInput;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

namespace Helicopter.Core
{
    public class GlobalInstaller : MonoInstaller<GlobalInstaller>
    {
        [SerializeField]
        private InputActionAsset _inputActionAsset;
        [SerializeField]
        private HelicopterView _helicopterView;
        [SerializeField]
        private HelicopterHUDView _helicopterHUDView;
        [SerializeField]
        private UIView _uiView;
        [SerializeField]
        private AudioManager _audioManager;
        
        public override void InstallBindings()
        {
            SignalBusInstaller.Install(Container);
            
            Container.BindInterfacesTo<PlayerInputService>().AsSingle().WithArguments(_inputActionAsset).NonLazy();
            
            BindHelicopter();
            BindHelicopterHUD();
            BindUI();
            BindGameService();

            BindAudioManager();
        }

        private void BindHelicopter()
        {
            Container.Bind<HelicopterSettings>().FromScriptableObjectResource("Configs/HelicopterSettings").AsSingle().NonLazy();
            Container.BindInterfacesTo<HelicopterController>().AsSingle().WithArguments(_helicopterView).NonLazy();
        }

        private void BindHelicopterHUD()
        {
            Container.BindInterfacesTo<HelicopterHUDPresenter>().AsSingle().WithArguments(_helicopterHUDView).NonLazy();
        }

        private void BindUI()
        {
            Container.BindInterfacesTo<UIFactory>().AsSingle().NonLazy();
            Container.BindInterfacesTo<UIController>().AsSingle().WithArguments(_uiView, 0.5f, 0.7f).NonLazy(); //TODO: Take values from local storage
        }

        private void BindGameService()
        {
            Container.DeclareSignal<ResetHelicopterRequestSignal>();
            Container.BindInterfacesTo<GameService>().AsSingle().NonLazy();
        }

        private void BindAudioManager()
        {
            Container.BindInterfacesTo<AudioManager>().FromInstance(_audioManager).AsSingle().NonLazy();
            Container.DeclareSignal<SetSoundVolumeSignal>();
            Container.DeclareSignal<SetMusicVolumeSignal>();
        }
    }
}