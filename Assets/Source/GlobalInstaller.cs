using Helicopter.Core.Scenes.Helicopter;
using Helicopter.Core.Scenes.Helicopter.HUD;
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
        
        public override void InstallBindings()
        {
            Container.BindInterfacesTo<PlayerInputService>().AsSingle().WithArguments(_inputActionAsset).NonLazy();
            
            BindHelicopter();
            BindHelicopterHUD();
            
        }

        private void BindHelicopter()
        {
            Container.Bind<HelicopterSettings>().FromScriptableObjectResource("Configs/HelicopterSettings").AsSingle().NonLazy();
            Container.BindInterfacesTo<HelicopterPresenter>().AsSingle().WithArguments(_helicopterView).NonLazy();
        }

        private void BindHelicopterHUD()
        {
            Container.BindInterfacesTo<HelicopterHUDPresenter>().AsSingle().WithArguments(_helicopterHUDView).NonLazy();
        }
    }
}