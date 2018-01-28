using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubPingController : MonoBehaviour
{
    /// <summary>Ping time in seconds</summary>
    public float PingTime;

    /// <summary>Ping range in meters.</summary>
    public float PingRange;

    /// <summary>Time that you can see people that have been identified.</summary>
    public float IdentifiedTime = 3;

    private bool isPinging;
    private float currentPingDist = -5000;
    private float pingStartTime;
    private IList<Taggable> hasTagged;

    private Sub sub;

	void Start ()
	{
	    sub = GetComponentInParent<Sub>();
	    hasTagged = new List<Taggable>();
	}
	
	void Update ()
    {
	    if (Input.GetButtonDown("Ping " + sub.Input) && !isPinging)
	    {
	        StartPing();
	    }
        else if (isPinging)
        {
            UpdatePing();
        }

        sub.Cam.PingDist = currentPingDist;
    }

    private void UpdatePing()
    {
        currentPingDist = ((Time.time - pingStartTime) / PingTime) * PingRange;

        foreach (var taggable in TaggableManager.Instance.AllTaggable)
        {
            if (taggable.gameObject == sub.gameObject)
                continue;

            // Pings originate from camera because I'm lazy
            var camPosition = sub.Cam.gameObject.transform.position;
            if (Vector3.Distance(camPosition, taggable.gameObject.transform.position) < currentPingDist)
            {
                // If it's just in range, cast a ray to see if we can see it
                RaycastHit hit;
                Physics.Raycast(new Ray(camPosition, taggable.gameObject.transform.position - camPosition), out hit);

                if (hit.rigidbody == taggable.Rigidbody)
                {
                    if (hasTagged.Contains(taggable) == false)
                    {
                        TaggableManager.Instance.Tag(taggable, sub.Player, IdentifiedTime);
                        hasTagged.Add(taggable);
                    }
                }
            }
        }

        if (Time.time - pingStartTime > PingTime)
        {
            PingEnded();
        }
    }

    private void PingEnded()
    {
        isPinging = false;
        pingStartTime = 0;
        sub.Cam.IsPinging = false;
        currentPingDist = -5000;
        hasTagged.Clear();
    }

    private void StartPing()
    {
        isPinging = true;
        pingStartTime = Time.time;
        currentPingDist = 0;
        sub.Cam.IsPinging = true;

        TaggableManager.Instance.TagForAllBut(sub.Taggable, sub.Player, IdentifiedTime);
    }
}
