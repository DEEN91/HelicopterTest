namespace Helicopter.Core.Scenes.Helicopter
{
    public interface IHelicopterController
    {
        HelicopterModel Model { get; }

        void Reset();
    }
}