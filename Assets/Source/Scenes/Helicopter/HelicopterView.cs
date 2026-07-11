using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Helicopter.Core.Scenes.Helicopter
{
    public class HelicopterView : MonoBehaviour
    {
        public event Action Exploded;
        
        [SerializeField] private Rigidbody _rigidbody;
        
        private float _engineForce;
        
        public bool IsOnGround { get; set; }
        
        private HelicopterModel _model;
        
        public Rigidbody Rigidbody => _rigidbody;

        private void OnCollisionEnter(Collision collision)
        {
            IsOnGround = true;
            
            if (collision.relativeVelocity.magnitude >= _model.CrashSpeed)
            {
                Exploded?.Invoke();
            }
        }

        private void OnCollisionExit()
        {
            IsOnGround = false;
        }

        public void ApplyModel(HelicopterModel model)
        {
            _model = model;
        }
    }
}