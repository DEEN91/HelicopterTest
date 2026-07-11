using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

namespace Helicopter.Core.Services.PlayerInput
{
    public class PlayerInputService : IPlayerInputService, ITickable
    {
        private InputAction _moveAction;
        private InputAction _rotateAction;
        private InputAction _liftAction;
        private InputAction _cameraAction;
        
        private readonly List<Action<Vector2>> _moveSubscriptions = new();
        private readonly List<Action<float>> _rotateSubscriptions = new();
        private readonly List<Action<float>> _liftSubscriptions = new();
        private readonly List<Action>_cameraChangedSubscriptions = new();

        private Vector2 _moveValue;
        private float _rotateValue;
        private float _liftValue;

        private Vector2 MoveValue
        {
            get => _moveValue;
            set
            {
                if(value.Equals(_moveValue))
                    return;

                foreach (var action in _moveSubscriptions)
                {
                    action?.Invoke(value);
                }
                
                _moveValue = value;
            }
        }

        private float RotateValue
        {
            get => _rotateValue;
            set
            {
                if(value.Equals(_rotateValue))
                    return;

                foreach (var action in _rotateSubscriptions)
                {
                    action?.Invoke(value);
                }
                
                _rotateValue = value;
            }
        }

        private float LiftValue
        {
            get => _liftValue;
            set
            {
                if(value.Equals(_liftValue))
                    return;

                foreach (Action<float> action in _liftSubscriptions)
                {
                    action?.Invoke(value);
                }
                
                _liftValue = value;
            }
        }

        public PlayerInputService(InputActionAsset inputActionAsset)
        {
            EnableHelicopterMap(inputActionAsset);
            EnableUIMap(inputActionAsset);
        }

        private void EnableHelicopterMap(InputActionAsset inputActionAsset)
        {
            var helicopterActionMap = inputActionAsset.FindActionMap("Helicopter");
            
            _moveAction = helicopterActionMap.FindAction("Move");
            _rotateAction = helicopterActionMap.FindAction("Tilt");
            _liftAction = helicopterActionMap.FindAction("Lift");
            
            helicopterActionMap.Enable();
        }

        private void EnableUIMap(InputActionAsset inputActionAsset)
        {
            var UIActionMap = inputActionAsset.FindActionMap("GameUI");
            _cameraAction = UIActionMap.FindAction("CameraSwitch");
            UIActionMap.Enable();
        }

        public void Tick()
        {
            MoveValue = _moveAction.ReadValue<Vector2>(); 
            RotateValue = _rotateAction.ReadValue<float>();
            LiftValue = _liftAction.ReadValue<float>();

            if (_cameraAction.IsPressed())
            {
                _cameraChangedSubscriptions.ForEach(x => x?.Invoke());
            }
        }

        public void SubscribeToMove(Action<Vector2> action)
        {
            _moveSubscriptions.Add(action);
        }

        public void SubscribeToRotate(Action<float> action)
        {
            _rotateSubscriptions.Add(action);
        }

        public void SubscribeToLift(Action<float> action)
        {
            _liftSubscriptions.Add(action);
        }

        public void SubscribeToCameraChange(Action action)
        {
            _cameraChangedSubscriptions.Add(action);
        }

        public void UnsubscribeFromMove(Action<Vector2> action)
        {
            _moveSubscriptions.Remove(action);
        }

        public void UnsubscribeFromRotate(Action<float> action)
        {
            _rotateSubscriptions.Remove(action);
        }

        public void UnsubscribeFromLift(Action<float> action)
        {
            _liftSubscriptions.Remove(action);
        }

        public void UnsubscribeToCameraChange(Action action)
        {
            _cameraChangedSubscriptions.Remove(action);
        }
    }
}