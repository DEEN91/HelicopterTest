using System;
using UnityEngine;
using UnityEngine.UI;

namespace Helicopter.Core.Scenes.UI.Popups
{
    public class InfoPopupView : MonoBehaviour
    {
        public event Action<InfoPopupView> Closed;
        
        [SerializeField]
        private Button _okButton;

        private void OnEnable()
        {
            _okButton.onClick.AddListener(OnOkButtonClicked);
        }

        private void OnDisable()
        {
            _okButton.onClick.RemoveListener(OnOkButtonClicked);
        }

        private void OnOkButtonClicked()
        {
            Close();
        }

        private void Close()
        {
            Closed?.Invoke(this);
        }
    }
}