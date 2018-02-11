using UnityEngine;
using UnityEngine.UI;

namespace Depth.UI
{
    public class AbilityIndicator : MonoBehaviour
    {
        public CooldownRadial Radial { get; private set; }

        private Image image;

        public void Awake()
        {
            Radial = GetComponentInChildren<CooldownRadial>();
            image = GetComponent<Image>();
        }

        public void SetIcon(Sprite icon)
        {
            image.sprite = icon;
        }
    }
}