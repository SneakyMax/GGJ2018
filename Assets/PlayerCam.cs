using UnityEngine;

public class PlayerCam : MonoBehaviour
{
    public Material DepthMaterial;
    public Texture IdentifiedTexture;
    public Texture IdentifiedDepthTexture;

    private Camera thisCamera;

    public bool IsPinging { get; set; }

    public float PingDist { get; set; }

    public float IdentifiedOpacity { get; set; }

    public float IdentifiedMaxOpacity = 1;

    private Material thisMaterial;

    void Awake()
    {
        thisMaterial = new Material(DepthMaterial);
        thisCamera = GetComponent<Camera>();
        thisCamera.depthTextureMode = DepthTextureMode.Depth;
    }

	void Start ()
    {
		
	}

    // From online
    void LateUpdate()
    {
        var p = GL.GetGPUProjectionMatrix(thisCamera.projectionMatrix, false);// Unity flips its 'Y' vector depending on if its in VR, Editor view or game view etc... (facepalm)
        p[2, 3] = p[3, 2] = 0.0f;
        p[3, 3] = 1.0f;
        var clipToWorld = Matrix4x4.Inverse(p * thisCamera.worldToCameraMatrix) * Matrix4x4.TRS(new Vector3(0, 0, -p[2, 2]), Quaternion.identity, Vector3.one);

        thisMaterial.SetMatrix("_ClipToWorld", clipToWorld);
    }

    void OnRenderImage(RenderTexture source, RenderTexture dest)
    {
        // Ping render
        Graphics.SetRenderTarget(dest);
        GL.LoadOrtho();
        thisMaterial.SetTexture("_MainTex", source);
        thisMaterial.SetFloat("_CurrentPingDist", PingDist);
        thisMaterial.SetTexture("_IdentifiedTex", IdentifiedTexture);
        thisMaterial.SetTexture("_IdentifiedDepth", IdentifiedDepthTexture);
        thisMaterial.SetFloat("_IdentifiedOpacity", IdentifiedOpacity * IdentifiedMaxOpacity);
        thisMaterial.SetPass(0);

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
