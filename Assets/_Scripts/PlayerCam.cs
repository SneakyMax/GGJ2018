using UnityEngine;

namespace Depth
{
    public class PlayerCam : MonoBehaviour
    {
        public Material DepthMaterial;
        private Camera thisCamera;

        public bool IsPinging { get; set; }

        public float PingDist { get; set; }

        private Material thisMaterial;
        private Material blitCopy;

        private RenderTexture targetTexture;
        private RenderTexture identifiedTexture;
        private RenderTexture identifiedDepthTexture;

        private Sub sub;

        private int rtWidth;
        private int rtHeight;

        private void Awake()
        {
            sub = GetComponentInParent<Sub>();

            thisMaterial = new Material(DepthMaterial);
            thisCamera = GetComponent<Camera>();
            thisCamera.depthTextureMode = DepthTextureMode.Depth;

            rtWidth = FixedSettings.Instance.RTWidth;
            rtHeight = FixedSettings.Instance.RTHeight;

            targetTexture = new RenderTexture(rtWidth, rtHeight, 24, RenderTextureFormat.ARGB32);
            thisCamera.targetTexture = targetTexture;

            blitCopy = new Material(Shader.Find("Hidden/BlitCopy"));
        }

        public void Start()
        {
            var identifiedEffect = GetComponentInChildren<IdentifiedEffect>();
            identifiedTexture = identifiedEffect.ColorTexture;
            identifiedDepthTexture = identifiedEffect.DepthTexture;

            RenderController.Instance.SetPlayerCam(sub.Player, targetTexture);

            var targetPosition = sub.gameObject.GetComponentInChildren<CamTargetPosition>().transform;
            transform.SetPositionAndRotation(targetPosition.position, targetPosition.rotation);
        }

        // From online
        public void LateUpdate()
        {
            // Allows getting world space coordinates from a depth texture and a camera position
            var p = GL.GetGPUProjectionMatrix(thisCamera.projectionMatrix, false);// Unity flips its 'Y' vector depending on if its in VR, Editor view or game view etc... (facepalm)
            p[2, 3] = p[3, 2] = 0.0f;
            p[3, 3] = 1.0f;
            var clipToWorld = Matrix4x4.Inverse(p * thisCamera.worldToCameraMatrix) * Matrix4x4.TRS(new Vector3(0, 0, -p[2, 2]), Quaternion.identity, Vector3.one);

            thisMaterial.SetMatrix("_ClipToWorld", clipToWorld);
        }

        public void OnRenderImage(RenderTexture source, RenderTexture dest)
        {
            // Ping render
            Graphics.SetRenderTarget(dest);
            thisMaterial.SetTexture("_MainTex", source);
            thisMaterial.SetFloat("_CurrentPingDist", PingDist);
            thisMaterial.SetTexture("_IdentifiedTex", identifiedTexture);
            thisMaterial.SetTexture("_IdentifiedDepth", identifiedDepthTexture);

            thisMaterial.SetPass(0);
            Helpers.FullscreenQuad();
        }
    }
}
