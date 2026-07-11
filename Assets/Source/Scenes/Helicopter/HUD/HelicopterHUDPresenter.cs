using System;
using Helicopter.Core.Managers.Camera;
using Helicopter.Core.Managers.Camera.Signals;
using Zenject;

namespace Helicopter.Core.Scenes.Helicopter.HUD
{
    public class HelicopterHUDPresenter : IInitializable, IDisposable
    {
        private readonly HelicopterHUDView _view;
        private readonly HelicopterHUDModel _model;
        
        [Inject]
        private IHelicopterController _helicopterController;
        [Inject]
        private HelicopterSettings _helicopterSettings;
        [Inject]
        private SignalBus _signalBus;
        
        private HelicopterModel _helicopterModel;

        public HelicopterHUDPresenter(HelicopterHUDView view)
        {
            _view = view;
            _model = new HelicopterHUDModel();
        }

        public void Initialize()
        {
            _helicopterModel = _helicopterController.Model;
            _model.SetHelicopterModel(_helicopterModel, _helicopterSettings);
       
            _view.ApplyModel(_model);
            _signalBus.Subscribe<CameraChangedSignal>(CameraChangedSignalHandler);
        }

        public void Dispose()
        {
            _signalBus.Unsubscribe<CameraChangedSignal>(CameraChangedSignalHandler);

        }

        private void CameraChangedSignalHandler(CameraChangedSignal signal)
        {
            _view.SetHeightAndHorizonActive(signal.Type == CameraType.Cockpit);
        }
    }
}