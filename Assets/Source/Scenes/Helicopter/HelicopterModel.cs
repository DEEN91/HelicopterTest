using UnityEngine;

namespace Helicopter.Core.Scenes.Helicopter
{
    public class HelicopterModel
    {
        private float _engineForce;
        private Vector2 _rotateValue;
        private Quaternion _localRotation;

        public Vector2 MoveValue { get; set; }
        public ref Vector2 RotateValue => ref _rotateValue;
        public ref Quaternion LocalRotation => ref _localRotation;
        public float Height { get; set; }

        public float CrashSpeed { get; set; }

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