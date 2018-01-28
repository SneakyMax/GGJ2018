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

    public FormattableText BlownUpText;

    public GameObject SpawnPoint;

    public int Lives;

    public float RespawnDelay = 3;

    public void Awake()
    {
        Taggable = GetComponent<Taggable>();
        Targetable = GetComponent<TorpedoTargetable>();

        Targetable.OnHitByTorpedo += BlowUp;
    }

    private void BlowUp(Torpedo torpedo)
    {
        BlowUp(torpedo.Parent.Player);
    }

    private void BlowUp(int playerCaused)
    {
        GetComponentInChildren<SubBody>().gameObject.SetActive(false);
        IsDestroyed = true;

        BlownUpText.gameObject.SetActive(true);
        BlownUpText.Format(playerCaused + 1);
        Lives--;

        StartCoroutine(Respawn());
    }

    private IEnumerator Respawn()
    {
        yield return new WaitForSeconds(RespawnDelay);
        IsDestroyed = false;
        transform.position = SpawnPoint.transform.position;
        transform.rotation = SpawnPoint.transform.rotation;
        GetComponentInChildren<SubBody>(true).gameObject.SetActive(true);
        BlownUpText.gameObject.SetActive(false);
    }

    public TorpedoTargetable Targetable { get; set; }

    public Taggable Taggable { get; private set; }

    public void Start()
    {
        SubManager.Instance.Subs.Add(this);
        BlownUpText.gameObject.SetActive(false);

        transform.position = SpawnPoint.transform.position;
        transform.rotation = SpawnPoint.transform.rotation;
    }

    public void OnDestroy()
    {
        SubManager.Instance.Subs.Remove(this);
    }
}