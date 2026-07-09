using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Helicopter.Core.Source
{
    public class HelicopterController : MonoBehaviour
    {
        [SerializeField] private InputActionAsset _inputActionAsset;

        [SerializeField] private Rigidbody _rigidbody;
        
        private InputAction _moveAction;
        private InputAction _rotateAction;
        private InputAction _liftAction;
        private InputActionMap _actionMap;

        private Vector2 _moveValue;
        private float _rotateValue;
        private float _liftValue;

        private float _engineForce;
        private Vector2 hTilt = Vector2.zero;
        private Vector2 hMove = Vector2.zero;
        private float hTurn = 0f;
        
        public float TurnForce = 3f;
        public float ForwardForce = 10f;
        public float ForwardTiltForce = 20f;
        public float TurnTiltForce = 30f;
        public float EffectiveHeight = 100f;
        public float turnForcePercent = 1.3f;

        public float turnTiltForcePercent = 1.5f;
        public bool IsOnGround { get; set; }
        
        float pitch;
        float roll;

        public float EngineForce
        {
            get { return _engineForce; }
            set
            {
                if (value < 0)
                    value = 0;

                // MainRotorController.RotarSpeed = value * 80;
                // SubRotorController.RotarSpeed = value * 40;
                // HelicopterSound.pitch = Mathf.Clamp(value / 40, 0, 1.2f);
                // if (UIGameController.runtime.EngineForceView != null)
                //     UIGameController.runtime.EngineForceView.text = string.Format("Engine value [ {0} ] ", (int)value);

                _engineForce = value;
            }
        }

        private void Awake()
        {
            _moveAction = _inputActionAsset.FindAction("Move");
            _rotateAction = _inputActionAsset.FindAction("Tilt");
            _liftAction = _inputActionAsset.FindAction("Lift");
        }

        private void OnEnable()
        {
            _actionMap = _inputActionAsset.FindActionMap("Helicopter");
            _actionMap.Enable();
        }

        private void OnDisable()
        {
            _actionMap.Disable();
        }

        private void Update()
        {
            _moveValue = _moveAction.ReadValue<Vector2>(); 
            _rotateValue = _rotateAction.ReadValue<float>();
            _liftValue = _liftAction.ReadValue<float>();
            
            Debug.Log("_moveValue: " + _moveValue + "_rotateValue: " + _rotateValue + "_liftValue: " + _liftValue);
        }

        private void FixedUpdate()
        {
            float tempY = 0;
            float tempX = 0;

            if (_liftValue > 0)
                EngineForce += 1f;
            else if(_liftValue < 0)
                EngineForce -= 1.6f;
            

            if (!IsOnGround)
            {
                if(_moveValue.x != 0)
                {
                    tempX = _moveValue.x>0 ? Time.fixedDeltaTime: -Time.fixedDeltaTime;
                }
                
                if(_moveValue.y != 0)
                {
                    tempY = _moveValue.y >0 ?Time.fixedDeltaTime : -Time.fixedDeltaTime;
                }
            }
            
            if (!IsOnGround&& _rotateValue != 0)
            {
                var force = _rotateValue*(turnForcePercent - Mathf.Abs(hMove.y)) * _rigidbody.mass;
                _rigidbody.AddRelativeTorque(0f, force, 0);
            }

            hMove.x += tempX;
            hMove.x = Mathf.Clamp(hMove.x, -1, 1);

            hMove.y += tempY;
            hMove.y = Mathf.Clamp(hMove.y, -1, 1);
            
            HandleLift();
            HandleMove();
            HandleTilt();
        }

        private void HandleMove()
        {
            var turn = TurnForce * Mathf.Lerp(hMove.x, hMove.x * (turnTiltForcePercent - Mathf.Abs(hMove.y)), Mathf.Max(0f, hMove.y));
            hTurn = Mathf.Lerp(hTurn, turn, Time.fixedDeltaTime * TurnForce);
            _rigidbody.AddRelativeTorque(0f, hTurn * _rigidbody.mass, 0f);
            _rigidbody.AddRelativeForce(Vector3.forward * Mathf.Max(0f, hMove.y * ForwardForce * _rigidbody.mass));
        }

        private void HandleLift()
        {
            var upForce = 1 - Mathf.Clamp(_rigidbody.transform.position.y / EffectiveHeight, 0, 1);
            upForce = Mathf.Lerp(0f, EngineForce, upForce) * _rigidbody.mass;
            _rigidbody.AddRelativeForce(Vector3.up * upForce);
        }
        
        
        private void HandleTilt()
        {
            hTilt.x = Mathf.Lerp(hTilt.x, hMove.x * TurnTiltForce, Time.deltaTime);
            hTilt.y = Mathf.Lerp(hTilt.y, hMove.y * ForwardTiltForce, Time.deltaTime);
            _rigidbody.transform.localRotation = Quaternion.Euler(hTilt.y, _rigidbody.transform.localEulerAngles.y, -hTilt.x);
        }
        
        private void OnCollisionEnter()
        {
            IsOnGround = true;
        }

        private void OnCollisionExit()
        {
            IsOnGround = false;
        }
    }
}