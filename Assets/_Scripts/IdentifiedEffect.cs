using System;
using UnityEngine;

namespace Depth
{
    public class IdentifiedEffect : MonoBehaviour
    {
        public RenderTexture ColorTexture { get; private set; }
        public RenderTexture DepthTexture { get; private set; }

        private Camera thisCamera;
        private Sub sub;

        private void Awake()
        {
            sub = GetComponentInParent<Sub>();
            thisCamera = GetComponent<Camera>();
            thisCamera.depthTextureMode = DepthTextureMode.Depth;

            thisCamera.cullingMask = LayerMask.GetMask(String.Format("Player {0} Identified", sub.Player + 1));

            // TODO one frame lag???
            ColorTexture = new RenderTexture(
                (int)FixedSettings.Instance.RenderTextureDimensions.x,
                (int)FixedSettings.Instance.RenderTextureDimensions.y,
                0,
                RenderTextureFormat.ARGB32);

            DepthTexture = new RenderTexture(
                (int)FixedSettings.Instance.RenderTextureDimensions.x,
                (int)FixedSettings.Instance.RenderTextureDimensions.y,
                24,
                RenderTextureFormat.Depth);

            thisCamera.SetTargetBuffers(ColorTexture.colorBuffer, DepthTexture.depthBuffer);
        }

        public void OnPreCull()
        {
            foreach (var info in TaggableManager.Instance.Tagged[sub.Player])
            {
                info.Taggable.Prepare(info);
            }
        }

        public void OnPostRender()
        {
            foreach (var info in TaggableManager.Instance.Tagged[sub.Player])
            {
                info.Taggable.Reset(info);
            }
        }
    }
}
