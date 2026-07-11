using UnityEngine;

namespace Helicopter.Core.Scenes.Helicopter.HUD.Components
{
    public class HorizonComponent : MonoBehaviour
    {
        [SerializeField] 
        private RectTransform horizonObject;
        [SerializeField]
        private float pixelsPerDegree = 6f;

        public void SetPitchAndRoll(float pitch, float roll)
        {
            transform.localRotation =
                Quaternion.Euler(0, 0, -roll);

            var pos = horizonObject.anchoredPosition;
            pos.y = -pitch * pixelsPerDegree;
            horizonObject.anchoredPosition = pos;
        }
    }
}