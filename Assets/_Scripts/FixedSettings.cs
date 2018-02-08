using UnityEngine;

namespace Depth
{
    public class FixedSettings : MonoBehaviour
    {
        public static FixedSettings Instance { get; private set; }

        public void Awake()
        {
            Instance = this;
        }

        public Vector2 RenderTextureDimensions;
    }
}
