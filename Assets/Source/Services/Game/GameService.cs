using System;
using Helicopter.Core.Scenes.Helicopter;
using Helicopter.Core.Services.Game.Signals;
using Zenject;

namespace Helicopter.Core.Services.Game
{
    public class GameService : IGameService, IInitializable, IDisposable
    {
        [Inject]
        private SignalBus SignalBus { get; set; }
        [Inject]
        private IHelicopterController HelicopterController { get; set; }

        public void Initialize()
        {
            SignalBus.Subscribe<ResetHelicopterRequestSignal>(ResetHelicopterRequestSignalHandler);
        }

        public void Dispose()
        {
            SignalBus.Unsubscribe<ResetHelicopterRequestSignal>(ResetHelicopterRequestSignalHandler);
        }

        private void ResetHelicopterRequestSignalHandler()
        {
            HelicopterController.Reset();
        }
    }
}