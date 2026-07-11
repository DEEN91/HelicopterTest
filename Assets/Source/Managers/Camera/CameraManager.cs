using System;
using Cinemachine;
using Helicopter.Core.Managers.Camera.Signals;
using Helicopter.Core.Services.PlayerInput;
using UnityEngine;
using Zenject;

namespace Helicopter.Core.Managers.Camera
{
    public class CameraManager : MonoBehaviour
    {
        [SerializeField]
        private CameraTypeToCamera[] _cameras;
        
        [Inject]
        private IPlayerInputService _playerInputService;
        [Inject]
        private SignalBus _signalBus;

        private int _cameraIndex;

        public int CameraIndex
        {
            get => _cameraIndex;
            set
            {
                if (value >= _cameras.Length)
                    value = 0;
                else if (value < 0)
                {
                    value = _cameras.Length - 1;
                }
                _cameraIndex = value;
            }
        }
        
        public CameraType CurrentCameraType => _cameras[CameraIndex].CameraType;

        private void OnEnable()
        {
            _playerInputService.SubscribeToCameraChange(OnCameraChangeHandler);
            UpdateCameras();
        }

        private void OnDisable()
        {
            _playerInputService.UnsubscribeToCameraChange(OnCameraChangeHandler);
        }

        private void OnCameraChangeHandler()
        {
            CameraIndex++;
            UpdateCameras();
        }

        private void UpdateCameras()
        {
            for (var i = 0; i < _cameras.Length; i++)
            {
                _cameras[i].Camera.Priority = i == CameraIndex ? 1 : 0;
            }
            var signal = new CameraChangedSignal()
            {
                Type =  CurrentCameraType
            };
            _signalBus.Fire(signal);
        }
    }

    [Serializable]
    public class CameraTypeToCamera
    {
        public CameraType CameraType;
        public CinemachineVirtualCamera Camera;
    }

    public enum CameraType
    {
        Action,
        Cockpit
    }
}