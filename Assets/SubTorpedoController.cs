using UnityEngine;

class SubTorpedoController : MonoBehaviour
{
    public float FireInterval = 1;

    public GameObject TorpedoPrefab;

    private float lastFireTime;

    private Sub sub;

    private Transform torpedoPoint;

    public void Awake()
    {
        sub = GetComponent<Sub>();
        torpedoPoint = GetComponentInChildren<TorpedoPoint>().transform;
    }

    public void Update()
    {
        if (Input.GetButtonDown("Torpedo " + sub.Input) && Time.time - lastFireTime > FireInterval)
        {
            lastFireTime = Time.time;
            FireTorpedo();
        }
    }

    private void FireTorpedo()
    {
        var newTorpedoObj = Instantiate(TorpedoPrefab, torpedoPoint.position, torpedoPoint.rotation);
        var torpedo = newTorpedoObj.GetComponent<Torpedo>();
        torpedo.Parent = sub;
    }
}