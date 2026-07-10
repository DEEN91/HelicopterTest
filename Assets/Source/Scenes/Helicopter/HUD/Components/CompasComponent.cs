using UnityEngine;
using UnityEngine.UI;

namespace Helicopter.Core.Scenes.Helicopter.HUD.Components
{
    public class CompasComponent : MonoBehaviour
    {
        [SerializeField]
        private RawImage _compasImage;

        public void SetCompasDirection(float value)
        {
            _compasImage.uvRect = new Rect(value, 0f, 1f, 1f);
        }
    }
}