using System;
using Cysharp.Threading.Tasks;
using Helicopter.Core.Services.PlayerInput;
using UnityEngine;
using Zenject;

namespace Helicopter.Core.Scenes.Helicopter
{
    public class HelicopterController : IInitializable, IDisposable, IFixedTickable, IHelicopterController
    {
        private readonly HelicopterView _view;
        public HelicopterModel Model { get; private set; }
       

        [Inject]
        private IPlayerInputService _playerInputService;
        [Inject]
        private readonly HelicopterSettings _settings;

        private float _liftValue;
        private Vector2 _moveValue;
        private float _rotateValue;
        
        private float _turnCache;
        private Vector3 _targetPos;
        private Quaternion _targetRot;

        public HelicopterController(HelicopterView view)
        {
            _view = view;
            Model = new HelicopterModel();
            _view.ApplyModel(Model);
            _targetPos = _view.transform.position;
            _targetRot = _view.transform.rotation;
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
        
        public async void Reset()
        {
            _view.Rigidbody.linearVelocity = Vector3.zero;
            _view.Rigidbody.angularVelocity = Vector3.zero;
            Model.EngineForce = 0;
            Model.MoveValue = Vector2.zero;
            Model.RotateValue = Vector2.zero;
            _liftValue = 0;
            _moveValue =  Vector2.zero;
            _rotateValue = 0;
            _turnCache = 0;
            await UniTask.WaitForFixedUpdate();

            _view.transform.position = _targetPos;
            _view.transform.rotation = _targetRot;
            await UniTask.WaitForFixedUpdate();

            _view.Rigidbody.linearVelocity = Vector3.zero;
            _view.Rigidbody.angularVelocity = Vector3.zero;
            Model.EngineForce = 0;
            Model.MoveValue = Vector2.zero;
            Model.RotateValue = Vector2.zero;
            _liftValue = 0;
            _moveValue =  Vector2.zero;
            _rotateValue = 0;
            _turnCache = 0;
            await UniTask.WaitForFixedUpdate();
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
                Model.EngineForce += 1f;
            else if(_liftValue < 0)
                Model.EngineForce -= 1.6f;
            

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
                var force = _rotateValue*(_settings.TurnForcePercent - Mathf.Abs(Model.MoveValue.y))*100f;
                _view.Rigidbody.AddRelativeTorque(0f, force, 0);
            }

            
            tempX = Mathf.Clamp(Model.MoveValue.x+tempX, -5, 5);
            tempY = Mathf.Clamp(Model.MoveValue.y+tempY, -5, 5);

            Model.MoveValue = new Vector2(tempX, tempY);
            
            HandleLift();
            HandleMove();
            HandleTilt();

            Model.Height = _view.transform.position.y;
        }
        
        private void HandleMove()
        {
            var turn = _settings.TurnForce * Mathf.Lerp(Model.MoveValue.x, Model.MoveValue.x * (_settings.TurnTiltForcePercent - Mathf.Abs(Model.MoveValue.y)), Mathf.Max(0f, Model.MoveValue.y));
            _turnCache = Mathf.Lerp(_turnCache, turn, Time.fixedDeltaTime * _settings.TurnForce);
            _view.Rigidbody.AddRelativeTorque(0f, _turnCache * 100f, 0f);
            _view.Rigidbody.AddRelativeForce(Vector3.forward * Mathf.Max(0f, Model.MoveValue.y * _settings.ForwardForce*100f));
        }

        private void HandleLift()
        {
            var upForce = 1 - Mathf.Clamp(_view.transform.position.y / _settings.EffectiveHeight, 0, 1);
            upForce = Mathf.Lerp(0f, Model.EngineForce, upForce)* 100f;
            _view.Rigidbody.AddRelativeForce(Vector3.up * upForce);
        }
        
        
        private void HandleTilt()
        {
            Model.RotateValue.x = Mathf.Lerp(Model.RotateValue.x, Model.MoveValue.x * _settings.TurnTiltForce, Time.deltaTime);
            Model.RotateValue.y = Mathf.Lerp(Model.RotateValue.y, Model.MoveValue.y * _settings.ForwardTiltForce, Time.deltaTime);


            var localRotation = Quaternion.Euler(Model.RotateValue.y, _view.transform.localEulerAngles.y, -Model.RotateValue.x);
            Model.LocalRotation = localRotation;
            _view.transform.localRotation = localRotation;
        }
    }
}