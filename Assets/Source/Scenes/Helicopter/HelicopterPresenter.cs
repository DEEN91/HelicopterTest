using System;
using Helicopter.Core.Services.PlayerInput;
using UnityEngine;
using Zenject;

namespace Helicopter.Core.Scenes.Helicopter
{
    public class HelicopterPresenter : IInitializable, IDisposable, IFixedTickable
    {
        private readonly HelicopterView _view;
        private readonly HelicopterModel _model;

        [Inject]
        private IPlayerInputService _playerInputService;
        [Inject]
        private readonly HelicopterSettings _settings;

        private float _liftValue;
        private Vector2 _moveValue;
        private float _rotateValue;
        
        private float _turnCache;

        public HelicopterPresenter(HelicopterView view)
        {
            _view = view;
            _model = new HelicopterModel();
            _view.ApplyModel(_model);
        }

        public void Initialize()
        {
            _playerInputService.SubscribeToLift(LiftHandler);
            _playerInputService.SubscribeToMove(MoveHandler);
            _playerInputService.SubscribeToRotate(RotateHandler);
        }

        public void Dispose()
        {
            _playerInputService.UnsubscribeFromLift(LiftHandler);
            _playerInputService.UnsubscribeFromMove(MoveHandler);
            _playerInputService.UnsubscribeFromRotate(RotateHandler);
        }

        private void MoveHandler(Vector2 obj)
        {
            _moveValue = obj;
        }

        private void LiftHandler(float obj)
        {
            _liftValue = obj;
        }

        private void RotateHandler(float obj)
        {
            _rotateValue = obj;
        }

        public void FixedTick()
        {
            float tempY = 0;
            float tempX = 0;

            if (_liftValue > 0)
                _model.EngineForce += 1f;
            else if(_liftValue < 0)
                _model.EngineForce -= 1.6f;
            

            if (!_view.IsOnGround)
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
            
            if (!_view.IsOnGround&& _rotateValue != 0)
            {
                var force = _rotateValue*(_settings.TurnForcePercent - Mathf.Abs(_model.MoveValue.y))*100f;
                _view.Rigidbody.AddRelativeTorque(0f, force, 0);
            }

            
            tempX = Mathf.Clamp(_model.MoveValue.x+tempX, -5, 5);
            tempY = Mathf.Clamp(_model.MoveValue.y+tempY, -5, 5);

            _model.MoveValue = new Vector2(tempX, tempY);
            
            HandleLift();
            HandleMove();
            HandleTilt();
        }
        
        private void HandleMove()
        {
            var turn = _settings.TurnForce * Mathf.Lerp(_model.MoveValue.x, _model.MoveValue.x * (_settings.TurnTiltForcePercent - Mathf.Abs(_model.MoveValue.y)), Mathf.Max(0f, _model.MoveValue.y));
            _turnCache = Mathf.Lerp(_turnCache, turn, Time.fixedDeltaTime * _settings.TurnForce);
            _view.Rigidbody.AddRelativeTorque(0f, _turnCache * 100f, 0f);
            _view.Rigidbody.AddRelativeForce(Vector3.forward * Mathf.Max(0f, _model.MoveValue.y * _settings.ForwardForce*100f));
        }

        private void HandleLift()
        {
            var upForce = 1 - Mathf.Clamp(_view.Rigidbody.transform.position.y / _settings.EffectiveHeight, 0, 1);
            upForce = Mathf.Lerp(0f, _model.EngineForce, upForce)* 100f;
            _view.Rigidbody.AddRelativeForce(Vector3.up * upForce);
        }
        
        
        private void HandleTilt()
        {
            var x = Mathf.Lerp(_model.RotateValue.x, _model.MoveValue.x * _settings.TurnTiltForce, Time.deltaTime);
            var y = Mathf.Lerp(_model.RotateValue.y, _model.MoveValue.y * _settings.ForwardTiltForce, Time.deltaTime);
            
            _model.RotateValue = new Vector2(x, y);
            _view.Rigidbody.transform.localRotation = Quaternion.Euler(_model.RotateValue.y, _view.transform.localEulerAngles.y, -_model.RotateValue.x);
        }
    }
}