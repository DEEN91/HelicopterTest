using System;
using Helicopter.Core.Scenes.Helicopter.Components;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Helicopter.Core.Scenes.Helicopter
{
    public class HelicopterView : MonoBehaviour
    {
        public event Action Exploded;
        
        [SerializeField]
        private Rigidbody _rigidbody;
        [SerializeField]
        private RotorComponent _topRotorComponent;
        [SerializeField]
        private RotorComponent _tailRotorComponent;
        
        private float _engineForce;
        
        public bool IsOnGround { get; set; }
        
        private HelicopterModel _model;

        public Rigidbody Rigidbody => _rigidbody;

        public void ApplyModel(HelicopterModel model)
        {
            _model = model;
        }

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

        private void Update()
        {
            if(_model == null)
                return;
            
            _topRotorComponent.RotorSpeed = _model.EngineForce;
            _tailRotorComponent.RotorSpeed = _model.EngineForce * 0.8f;
            Debug.Log(@"Engine force" + _model.EngineForce);
        }
    }
}