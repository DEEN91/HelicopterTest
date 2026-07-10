using System.Numerics;

namespace Helicopter.Core.Scenes.Helicopter.HUD
{
    public class HelicopterHUDModel
    {
        private HelicopterModel _helicopterModel;

        public float ViewDirection
        {
        get
            {
                if (_helicopterModel == null)
                    return 0f;
                
                return _helicopterModel.LocalRotation.eulerAngles.y/360f;
            }
        }

        public void SetHelicopterModel(HelicopterModel helicopterModel)
        {
            _helicopterModel = helicopterModel;
        }
    }
}