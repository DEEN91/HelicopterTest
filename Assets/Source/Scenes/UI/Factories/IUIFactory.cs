using System;
using Cysharp.Threading.Tasks;
using Helicopter.Core.Scenes.UI.Popups;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Helicopter.Core.Scenes.UI.Factories
{
    public interface IUIFactory
    {
        UniTask<InfoPopupView> CreateInfoPopup(Transform viewTransform);
        void ReturnInfoPopup(InfoPopupView infoPopupView);
    }

    public class UIFactory : IUIFactory
    {
        private const string InfoPopupViewPath = "UI/InfoPanel.prefab";
        
        public async UniTask<InfoPopupView> CreateInfoPopup(Transform viewTransform)
        {
            var gameObject= await Addressables.InstantiateAsync(InfoPopupViewPath, viewTransform);

            return gameObject.GetComponent<InfoPopupView>();
        }

        public void ReturnInfoPopup(InfoPopupView infoPopupView)
        {
            Addressables.Release(infoPopupView.gameObject);
        }
    }
}