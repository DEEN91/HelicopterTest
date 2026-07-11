namespace Helicopter.Core.Managers.Audio.Signals
{
    public class SetSoundVolumeSignal
    {
        public float Value { get; }

        public SetSoundVolumeSignal(float value)
        {
            Value = value;
        }
    }
}