using System.Collections;
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
    private Coroutine fadeOutIdentifiedCoroutine;

    private Sub sub;

	void Start ()
	{
	    sub = GetComponentInParent<Sub>();
	}
	
	void Update ()
    {
	    if (Input.GetKeyDown(KeyCode.Space) && !isPinging)
	    {
	        isPinging = true;
	        pingStartTime = Time.time;
	        currentPingDist = 0;
	        sub.Cam.IsPinging = true;
	        sub.Cam.IdentifiedOpacity = 1;
	        if (fadeOutIdentifiedCoroutine != null)
	            StopCoroutine(fadeOutIdentifiedCoroutine);
	        TaggableManager.Instance.Clear(sub.Player);
	    }
        else if (isPinging)
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
                        TaggableManager.Instance.Tag(taggable, sub.Player);
                    }
                }
            }

            if (Time.time - pingStartTime > PingTime)
            {
                isPinging = false;
                pingStartTime = 0;
                sub.Cam.IsPinging = false;
                currentPingDist = -5000;

                FadeOutIdentified();
            }
        }

        sub.Cam.PingDist = currentPingDist;
    }

    private void FadeOutIdentified()
    {
        fadeOutIdentifiedCoroutine = StartCoroutine(FadeOutIdentified_Coroutine());
    }

    private IEnumerator FadeOutIdentified_Coroutine()
    {
        var startTime = Time.time;
        while (Time.time - startTime < IdentifiedTime)
        {
            var opacity = 1.0f - (Time.time - startTime) / IdentifiedTime;
            sub.Cam.IdentifiedOpacity = opacity;
            yield return new WaitForEndOfFrame();
        }
        TaggableManager.Instance.Clear(sub.Player);
    }
}
