using UnityEngine;

namespace Depth
{
    public class FixedSettings : MonoBehaviour
    {
        public static FixedSettings Instance { get; private set; }

        public int RTWidth { get { return (int)RenderTextureDimensions.x; } }
        public int RTHeight { get { return (int)RenderTextureDimensions.y; } }

        public Vector2 RenderTextureDimensions;
        
        public void Awake()
        {
            Instance = this;
        }

    }
}
