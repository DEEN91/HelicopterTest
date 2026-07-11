namespace Helicopter.Core.Managers.Audio.Signals
{
    public class SetMusicVolumeSignal
    {
        public float Value { get; }

        public SetMusicVolumeSignal(float value)
        {
            Value = value;
        }
    }
}