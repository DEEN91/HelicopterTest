using Zenject;

namespace Helicopter.Core.Scenes.Helicopter.HUD
{
    public class HelicopterHUDPresenter : IInitializable
    {
        private readonly HelicopterHUDView _view;
        private readonly HelicopterHUDModel _model;
        
        [Inject]
        private IHelicopterPresenter _helicopterPresenter;
        
        private HelicopterModel _helicopterModel;
        

        public HelicopterHUDPresenter(HelicopterHUDView view)
        {
            _view = view;
            _model = new HelicopterHUDModel();
            _view.ApplyModel(_model);
        }

        public void Initialize()
        {
            _helicopterModel = _helicopterPresenter.Model;
            _model.SetHelicopterModel(_helicopterModel);
        }
    }
}