using UnityEngine;
using UnityEngine.InputSystem;

namespace Helicopter.Core.Scenes.Helicopter
{
    public class HelicopterView : MonoBehaviour
    {
        [SerializeField] private Rigidbody _rigidbody;
        
        private float _engineForce;
        
        public bool IsOnGround { get; set; }
        
        private HelicopterModel _model;
        
        public Rigidbody Rigidbody => _rigidbody;

        private void OnCollisionEnter()
        {
            IsOnGround = true;
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