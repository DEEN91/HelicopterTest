using UnityEngine;

namespace Helicopter.Core.Scenes.Helicopter
{
    [CreateAssetMenu(fileName = "HelicopterSettings", menuName = "Helicopter/Helicopter Settings")]
    public class HelicopterSettings : ScriptableObject
    {
        public float TurnForcePercent = 1.3f;
        public float TurnForce = 5f;
        public float ForwardForce = 10f;
        public float ForwardTiltForce = 20f;
        public float TurnTiltForce = 30f;
        public float EffectiveHeight = 100f;
        public float TurnTiltForcePercent = 1.5f;
        public float CrashSpeed = 12f;
    }
}