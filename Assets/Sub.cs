using System.Collections;
using System.Collections.Generic;
using Assets;
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

    public bool IsDestroyed { get; private set; }

    public void Awake()
    {
        Taggable = GetComponent<Taggable>();
        Targetable = GetComponent<TorpedoTargetable>();

        Targetable.OnHitByTorpedo += BlowUp;
    }

    private void BlowUp()
    {
        Destroy(GetComponentInChildren<SubBody>().gameObject);
        IsDestroyed = true;
    }

    public TorpedoTargetable Targetable { get; set; }

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
