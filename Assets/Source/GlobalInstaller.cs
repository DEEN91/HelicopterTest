using Helicopter.Core.Scenes.Helicopter;
using Helicopter.Core.Services.PlayerInput;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

namespace Helicopter.Core
{
    public class GlobalInstaller : MonoInstaller<GlobalInstaller>
    {
        [SerializeField]
        private InputActionAsset inputActionAsset;
        [SerializeField]
        private HelicopterView _helicopterView;
        
        
        public override void InstallBindings()
        {
            Container.BindInterfacesTo<PlayerInputService>().AsSingle().WithArguments(inputActionAsset).NonLazy();
            
            Container.Bind<HelicopterSettings>().FromScriptableObjectResource("Configs/HelicopterSettings").AsSingle().NonLazy();
            Container.BindInterfacesTo<HelicopterPresenter>().AsSingle().WithArguments(_helicopterView).NonLazy();
        }
    }
}