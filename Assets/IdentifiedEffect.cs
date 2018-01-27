using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdentifiedEffect : MonoBehaviour
{
    public Material DepthOnly;
    public RenderTexture DepthOnlyTexture;
    private Camera thisCamera;
    private Sub sub;

    public float Opacity;

    void Awake()
    {
        sub = GetComponentInParent<Sub>();
        thisCamera = GetComponent<Camera>();
        thisCamera.depthTextureMode = DepthTextureMode.Depth;
    }

    public void OnPreCull()
    {
        foreach (var tagged in TaggableManager.Instance.Tagged[sub.Player])
        {
            tagged.SetLayer(sub.Player);
        }
    }

    public void OnPostRender()
    {
        foreach (var tagged in TaggableManager.Instance.Tagged[sub.Player])
        {
            tagged.ResetLayer();
        }
    }

    public void OnRenderImage(RenderTexture src, RenderTexture dest)
    {
        Graphics.SetRenderTarget(dest);
        GL.LoadOrtho();
        DepthOnly.SetTexture("_MainTex", src);
        DepthOnly.SetPass(0);

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
