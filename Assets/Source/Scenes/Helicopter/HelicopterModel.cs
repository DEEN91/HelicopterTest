using UnityEngine;

namespace Helicopter.Core.Scenes.Helicopter
{
    public class HelicopterModel
    {
        private float _engineForce;
        
        public Vector2 MoveValue { get; set; }
        public Vector2 RotateValue { get; set; }
        public float EngineForce
        {
            get { return _engineForce; }
            set
            {
                if (value < 0)
                    value = 0;
                _engineForce = value;
            }
        }
    }
}