using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaggableManager : MonoBehaviour
{
    public static TaggableManager Instance { get; private set; }

    public IList<IList<Taggable>> Tagged;

    public IList<Taggable> AllTaggable;

    public TaggableManager()
    {
        Tagged = new List<IList<Taggable>>();
        Tagged.Add(new List<Taggable>());
        Tagged.Add(new List<Taggable>());
        Tagged.Add(new List<Taggable>());
        Tagged.Add(new List<Taggable>());

        AllTaggable = new List<Taggable>();
    }

    private void Awake()
    {
        Instance = this;
    }

    public void Tag(Taggable taggable, int player)
    {
        Tagged[player].Add(taggable);
    }

    public void Clear(int player)
    {
        Tagged[player].Clear();
    }

    public void Add(Taggable taggable)
    {
        AllTaggable.Add(taggable);
    }

    public void Remove(Taggable taggable)
    {
        AllTaggable.Remove(taggable);
    }
}
