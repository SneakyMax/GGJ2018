using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Taggable : MonoBehaviour
{
    public Rigidbody Rigidbody { get; private set; }

    public Material TagMaterial;

    private int[] layers =
    {
        8, 9, 10, 11
    };

    private IList<TaggableRenderer> subRenderers;

    void Awake()
    {
        subRenderers = new List<TaggableRenderer>();
        Rigidbody = GetComponentInChildren<Rigidbody>();
    }

	void Start ()
	{
	    TaggableManager.Instance.Add(this);

	    foreach (var childMeshRenderer in GetComponentsInChildren<MeshRenderer>())
	    {
	        var subRenderer = childMeshRenderer.gameObject.AddComponent<TaggableRenderer>();
	        subRenderer.TagMaterial = TagMaterial;
	        subRenderers.Add(subRenderer);
	    }
	}

    void Update ()
    {
		
	}

    public void SetLayer(int player)
    {
        var layer = layers[player];
        foreach (var renderable in GetComponentsInChildren<MeshFilter>())
        {
            renderable.gameObject.layer = layer;
        }
    }

    public void ResetLayer()
    {
        foreach (var renderable in GetComponentsInChildren<MeshFilter>())
        {
            renderable.gameObject.layer = 0;
        }
    }

    public void ResetMaterials()
    {
        foreach (var subRenderer in subRenderers)
        {
            subRenderer.Reset();
        }
    }

    public void OverwriteMaterials(float opacity)
    {
        foreach (var subRenderer in subRenderers)
        {
            subRenderer.SetTag(opacity);
        }
    }

    public void Reset(TaggableManager.TagInfo info)
    {
        ResetLayer();
        ResetMaterials();
    }

    public void Prepare(TaggableManager.TagInfo info)
    {
        SetLayer(info.Player);

        var opacity = 1.0f - Mathf.Clamp01((Time.time - info.StartTime) / info.Duration);

        OverwriteMaterials(opacity);
    }
}
