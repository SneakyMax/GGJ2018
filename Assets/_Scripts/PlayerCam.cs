using UnityEngine;

namespace Depth
{
    public class PlayerCam : MonoBehaviour
    {
        public Material DepthMaterial;
        public Texture IdentifiedTexture;
        public Texture IdentifiedDepthTexture;

        private Camera thisCamera;

        public bool IsPinging { get; set; }

        public float PingDist { get; set; }

        public float IdentifiedMaxOpacity = 1;

        public Material ThisMaterial;

        private void Awake()
        {
            ThisMaterial = new Material(DepthMaterial);
            thisCamera = GetComponent<Camera>();
            thisCamera.depthTextureMode = DepthTextureMode.Depth;
        }

        public void Start ()
        {
		
        }

        // From online
        public void LateUpdate()
        {
            // Allows getting world space coordinates from a depth texture and a camera position
            var p = GL.GetGPUProjectionMatrix(thisCamera.projectionMatrix, false);// Unity flips its 'Y' vector depending on if its in VR, Editor view or game view etc... (facepalm)
            p[2, 3] = p[3, 2] = 0.0f;
            p[3, 3] = 1.0f;
            var clipToWorld = Matrix4x4.Inverse(p * thisCamera.worldToCameraMatrix) * Matrix4x4.TRS(new Vector3(0, 0, -p[2, 2]), Quaternion.identity, Vector3.one);

            ThisMaterial.SetMatrix("_ClipToWorld", clipToWorld);
        }

        public void OnRenderImage(RenderTexture source, RenderTexture dest)
        {
            // Ping render
            Graphics.SetRenderTarget(dest);
            GL.LoadOrtho();
            ThisMaterial.SetTexture("_MainTex", source);
            ThisMaterial.SetFloat("_CurrentPingDist", PingDist);
            ThisMaterial.SetTexture("_IdentifiedTex", IdentifiedTexture);
            ThisMaterial.SetTexture("_IdentifiedDepth", IdentifiedDepthTexture);
            ThisMaterial.SetPass(0);

            GL.Begin(GL.QUADS);
            GL.Color(Color.white);
            GL.TexCoord2(0, 0);
            GL.Vertex3(0, 0, 0.1f);

            GL.TexCoord2(1, 0);
            GL.Vertex3(1, 0, 0.1f);

            GL.TexCoord2(1, 1);
            GL.Vertex3(1, 1, 0.1f);

            GL.TexCoord2(0, 1);
            GL.Vertex3(0, 1, 0.1f);
            GL.End();
        }
    }
}
