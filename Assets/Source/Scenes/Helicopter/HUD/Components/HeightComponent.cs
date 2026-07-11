using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Helicopter.Core.Scenes.Helicopter.HUD.Components
{
    public class HeightComponent : MonoBehaviour
    {
        [SerializeField]
        private Slider _heightSlider;
        
        [SerializeField]
        private TMP_Text _heightText;
        
        private float _maxHeight;

        public void SetMaxHeight(float value)
        {
            _maxHeight = value;
        }

        public void SetCurrentHeight(int value)
        {
            _heightText.text = value.ToString();
            
            var percentage = value*1f / _maxHeight;
            _heightSlider.value = percentage;
        }
    }
}