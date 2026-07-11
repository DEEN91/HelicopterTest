using UnityEngine;
using UnityEngine.UI;

namespace Helicopter.Core.Utils
{
    public class ButtonWithSlider : Button
    {
        [SerializeField]
        private Slider _slider;
        
        public float SliderValue => _slider.value;
        public bool SliderActive => _slider.isActiveAndEnabled;
        
        // ReSharper disable once InconsistentNaming
        public Slider.SliderEvent onSliderValueChanged => _slider.onValueChanged;
        
        public void SetSliderActive(bool active)
        {
            _slider.gameObject.SetActive(active);
        }
        
        public void SetSliderValue(float value)
        {
            _slider.value = value;
        }
        
        public void SetSliderWithoutNotify(float value)
        {
            _slider.SetValueWithoutNotify(value);
        }
    }
}