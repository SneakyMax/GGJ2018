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

        private Material postProcessMaterial;

        private void Awake()
        {
            sub = GetComponentInParent<Sub>();
            thisCamera = GetComponent<Camera>();
            
            ColorTexture = new RenderTexture(FixedSettings.Instance.RTWidth, FixedSettings.Instance.RTHeight, 0, RenderTextureFormat.ARGB32);
            DepthTexture = new RenderTexture(FixedSettings.Instance.RTWidth, FixedSettings.Instance.RTHeight, 24, RenderTextureFormat.Depth);

            postProcessMaterial = new Material(Shader.Find("Custom/PingPostProcessing"));
            
            // CAUSES CAMERA TO BE RENDERED LAST
            // thisCamera.SetTargetBuffers(ColorTexture.colorBuffer, DepthTexture.depthBuffer);
        }

        public void Start()
        {
            thisCamera.targetTexture = ColorTexture;
            thisCamera.depthTextureMode = DepthTextureMode.Depth;
            thisCamera.cullingMask = LayerMask.GetMask(String.Format("Player {0} Identified", sub.Player + 1));
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

        public void OnRenderImage(RenderTexture src, RenderTexture dest)
        {
            Graphics.SetRenderTarget(DepthTexture);
            postProcessMaterial.SetTexture("_MainTex", src);
            postProcessMaterial.SetPass(1);
            Helpers.FullscreenQuad();

            Graphics.SetRenderTarget(dest);
            postProcessMaterial.SetTexture("_MainTex", src);
            postProcessMaterial.SetPass(0);
            Helpers.FullscreenQuad();
        }
    }
}
