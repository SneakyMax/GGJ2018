using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sub : MonoBehaviour
{
    public PlayerCam Cam;

    /// <summary>
    /// 0-indexed
    /// </summary>
    public int Player;

    public string Input
    {
        get { return "P" + (Player + 1); }
    }

    public Transform Transform
    {
        get { return transform; }
    }

    public void Awake()
    {
        Taggable = GetComponent<Taggable>();
    }

    public Taggable Taggable { get; private set; }

    public void Start()
    {
        SubManager.Instance.Subs.Add(this);
    }

    public void OnDestroy()
    {
        SubManager.Instance.Subs.Remove(this);
    }
}
