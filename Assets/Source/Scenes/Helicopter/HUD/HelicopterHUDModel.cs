using System.Numerics;

namespace Helicopter.Core.Scenes.Helicopter.HUD
{
    public class HelicopterHUDModel
    {
        private HelicopterModel _helicopterModel;

        public int CurrentHeight
        {
            get
            {
                if (_helicopterModel == null)
                    return 0;

                return (int)_helicopterModel.Height;
            }
        }

        public float ViewDirection
        {
            get
            {
                if (_helicopterModel == null)
                    return 0f;
                
                return _helicopterModel.LocalRotation.eulerAngles.y/360f;
            }
        }

        public float Pitch
        {
            get
            {
                if (_helicopterModel == null)
                    return 0f;
                
                return Normalize(_helicopterModel.LocalRotation.eulerAngles.x);
            }
        }

        public float Roll
        {
            get
            {
                if (_helicopterModel == null)
                    return 0f;
                
                return Normalize(_helicopterModel.LocalRotation.eulerAngles.z);
            }
        }

        public float EffectiveHeight { get; private set; }

        public void SetHelicopterModel(HelicopterModel helicopterModel, HelicopterSettings helicopterSettings)
        {
            _helicopterModel = helicopterModel;
            EffectiveHeight = helicopterSettings.EffectiveHeight;
        }

        private static float Normalize(float angle)
        {
            if (angle > 180)
                angle -= 360;

            return angle;
        }
    }
}