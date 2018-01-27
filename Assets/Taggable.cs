using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Taggable : MonoBehaviour
{
    public Rigidbody Rigidbody { get; private set; }

    private int[] layers =
    {
        8, 9, 10, 11
    };

    void Awake()
    {
        Rigidbody = GetComponentInChildren<Rigidbody>();
    }

	void Start ()
	{
	    TaggableManager.Instance.Add(this);
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
}
