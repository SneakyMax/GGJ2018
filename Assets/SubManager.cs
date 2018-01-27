using System.Collections.Generic;
using Assets;
using UnityEngine;

public class SubManager : MonoBehaviour
{
    public static SubManager Instance { get; private set; }

    public IList<Sub> Subs { get; private set; }

    public IList<TorpedoTargetable> Targetable { get; private set; }

    public SubManager()
    {
        Subs = new List<Sub>();
        Targetable = new List<TorpedoTargetable>();
    }

    public void Awake()
    {
        Instance = this;
    }
}