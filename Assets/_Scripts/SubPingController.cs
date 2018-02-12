using System.Collections.Generic;
using UnityEngine;
using XInputDotNetPure;

namespace Depth
{
    public class SubPingController : MonoBehaviour
    {
        private bool isPinging;
        private float currentPingDist = -5000;
        private float pingStartTime;
        private IList<Taggable> hasTagged;

        private Sub sub;

        private void Start ()
        {
            sub = GetComponentInParent<Sub>();
            hasTagged = new List<Taggable>();
        }

        private void Update ()
        {
            if (sub.IsDestroyed)
                return;

            if (!GameplayManager.Instance.AllowInput)
                return;

            if (sub.InputState.Buttons.A == ButtonState.Pressed && !isPinging)
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
            var pingPercentFinished = (Time.time - pingStartTime) / sub.Parameters.PingTime;
            currentPingDist = pingPercentFinished * sub.Parameters.PingRange;

            foreach (var taggable in TaggableManager.Instance.AllTaggable)
            {
                if (taggable == null)
                    continue; // destroyed

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
                            TaggableManager.Instance.Tag(taggable, sub.Player, sub.Parameters.IdentifiedTime);
                            SoundManager.PlaySound("target_aquired", 0.25f);
                            hasTagged.Add(taggable);
                        }
                    }
                }
            }

            sub.Panel.PingIndicator.Radial.Percent = 1.0f - pingPercentFinished;

            if (Time.time - pingStartTime > sub.Parameters.PingTime)
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

            sub.Panel.PingIndicator.Radial.Percent = 0;
        }

        private void StartPing()
        {
            isPinging = true;
            pingStartTime = Time.time;
            currentPingDist = 0;
            sub.Cam.IsPinging = true;

            TaggableManager.Instance.TagForAllBut(sub.Taggable, sub.Player, sub.Parameters.IdentifiedTime);
            sub.Panel.PingIndicator.Radial.Percent = 1;

            SoundManager.PlaySound("Sonar", (float)0.8);

            sub.Body.Flash();
        }
    }
}
