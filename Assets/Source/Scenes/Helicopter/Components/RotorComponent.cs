using UnityEngine;

namespace Helicopter.Core.Scenes.Helicopter.Components
{
    public class RotorComponent : MonoBehaviour
    {
        private enum Axis
        {
            X, Y, Z
        }
        
        [SerializeField]
        private Axis _axis;
        [SerializeField]
        private Transform _rotor;
        [SerializeField] 
        private AudioSource _audioSource;

        private float _rotorSpeed;
        private float _rotateDegree;
        private Vector3 _originalRotate;

        public float RotorSpeed
        {
            get => _rotorSpeed;
            set
            {
                _audioSource.pitch = Mathf.Lerp(_audioSource.pitch, value/1000f, Time.deltaTime);
                _rotorSpeed = value;
            }
        }

        private void Start ()
        {
            _originalRotate = transform.localEulerAngles;
        }

        private void Update ()
        {
            _rotateDegree += RotorSpeed * Time.deltaTime;
            _rotateDegree %= 360;

            transform.localRotation = _axis switch
            {
                Axis.Y => Quaternion.Euler(_originalRotate.x, _rotateDegree, _originalRotate.z),
                Axis.Z => Quaternion.Euler(_originalRotate.x, _originalRotate.y, _rotateDegree),
                _ => Quaternion.Euler(_rotateDegree, _originalRotate.y, _originalRotate.z)
            };
        }
    }
}