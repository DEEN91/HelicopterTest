using System;
using Helicopter.Core.Scenes.Helicopter.HUD.Components;
using UnityEngine;

namespace Helicopter.Core.Scenes.Helicopter.HUD
{
    public class HelicopterHUDView : MonoBehaviour
    {
        [SerializeField]
        private CompasComponent _compasComponent;
        [SerializeField]
        private HeightComponent _heightComponent;
        [SerializeField]
        private HorizonComponent _horizonComponent;

        private HelicopterHUDModel _model;


        public void ApplyModel(HelicopterHUDModel helicopterHUDModel)
        {
            _heightComponent.SetMaxHeight(helicopterHUDModel.EffectiveHeight);
            _model =  helicopterHUDModel;
        }

        private void Update()
        {
            if(_model == null)
                return;
            
            _compasComponent.SetCompasDirection(_model.ViewDirection);
            _heightComponent.SetCurrentHeight(_model.CurrentHeight);
            _horizonComponent.SetPitchAndRoll(_model.Pitch, _model.Roll);
        }

        public void SetHeightAndHorizonActive(bool value)
        {
            _heightComponent.gameObject.SetActive(value);
            _horizonComponent.gameObject.SetActive(value);
        }
    }
}