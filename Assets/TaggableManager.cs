using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets;
using UnityEngine;

public class TaggableManager : MonoBehaviour
{
    public class TagInfo
    {
        public Taggable Taggable;
        public float StartTime;
        public float Duration;
        public int Player;
        public Coroutine RemoveCoroutine;
    }

    public static TaggableManager Instance { get; private set; }

    public IList<IList<TagInfo>> Tagged;

    public IList<Taggable> AllTaggable;

    public TaggableManager()
    {
        Tagged = new List<IList<TagInfo>>
        {new List<TagInfo>(), new List<TagInfo>(), new List<TagInfo>(), new List<TagInfo>()};

        AllTaggable = new List<Taggable>();
    }

    private void Awake()
    {
        Instance = this;
    }

    public void Tag(Taggable taggable, int player, float duration)
    {
        var existing = Tagged[player].FirstOrDefault(x => x.Taggable == taggable);
        if (existing != null)
        {
            Remove(existing);
        }

        Debug.Log(String.Format("Tagging {0} for player {1}", taggable.gameObject.name, player));

        var info = new TagInfo
        {
            Taggable = taggable,
            Duration = duration,
            StartTime = Time.time,
            Player = player
        };
        Tagged[player].Add(info);

        info.RemoveCoroutine = StartCoroutine(RemoveAfterDurationCoroutine(info));

        Helpers.Instance.ShowPing(SubManager.Instance.GetSub(player), taggable.transform.position);
    }

    public void Remove(TagInfo tagInfo)
    {
        if (tagInfo.RemoveCoroutine != null)
            StopCoroutine(tagInfo.RemoveCoroutine);
        Tagged[tagInfo.Player].Remove(tagInfo);
    }

    public IEnumerator RemoveAfterDurationCoroutine(TagInfo info)
    {
        yield return new WaitForSeconds(info.Duration);
        Remove(info);
    }

    public void Add(Taggable taggable)
    {
        AllTaggable.Add(taggable);
    }

    public void Remove(Taggable taggable)
    {
        AllTaggable.Remove(taggable);

        var toRemove = Tagged.SelectMany(x => x.Where(y => y.Taggable == taggable)).ToList();

        foreach (var info in toRemove)
        {
            Remove(info);
        }
    }

    public void TagForAllBut(Taggable taggable, int player, float duration)
    {
        for (var i = 0; i < Tagged.Count; i++)
        {
            if (i == player)
                continue;
            Tag(taggable, i, duration);
        }
    }
}
