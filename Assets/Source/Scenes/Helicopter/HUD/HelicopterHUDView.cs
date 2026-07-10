using System;
using Helicopter.Core.Scenes.Helicopter.HUD.Components;
using UnityEngine;

namespace Helicopter.Core.Scenes.Helicopter.HUD
{
    public class HelicopterHUDView : MonoBehaviour
    {
        [SerializeField]
        private CompasComponent _compasComponent;

        private HelicopterHUDModel _model;


        public void ApplyModel(HelicopterHUDModel helicopterHUDModel)
        {
            _model =  helicopterHUDModel;
        }

        private void Update()
        {
            if(_model == null)
                return;
            
            _compasComponent.SetCompasDirection(_model.ViewDirection);
        }
    }
}