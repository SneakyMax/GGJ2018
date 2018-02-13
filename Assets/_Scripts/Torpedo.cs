using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;

namespace Depth
{
    public class Torpedo : MonoBehaviour, ICanLockOn
    {
        public float InitialSpeed = 1;

        public float Acceleration = 1;

        public float MaxSpeed = 10;

        public float HomingConeHalfAngle = 10;

        public float HomingTurnRate = 1;

        public float MaxLifetime = 10;

        public float HomingRange = 30;

        public float ArmTime = 1;

        public Sub Parent { get; set; }

        public GameObject ExplosionPrefab;

        private Rigidbody body;
        private Collider collider;

        public CanBeLockedOnTo Target;

        private float speed;
        private Vector3 direction;
        private bool armed;

        public void Awake()
        {
            body = GetComponent<Rigidbody>();
            collider = GetComponentInChildren<Collider>();
        }

        public void Start()
        {
            collider.enabled = false;
            speed = InitialSpeed + Parent.GetComponent<Rigidbody>().velocity.magnitude;
            direction = transform.forward;

            foreach (var component in GetComponents<Collider>())
            {
                component.enabled = false;
            }

            StartCoroutine(EnableCollision());
            StartCoroutine(BlowUpAfterTime());
            StartCoroutine(ArmAfter());
        }

        private IEnumerator BlowUpAfterTime()
        {
            yield return new WaitForSeconds(MaxLifetime);
            Explode();
        }

        private IEnumerator EnableCollision()
        {
            yield return new WaitForSeconds(0.25f);
            collider.enabled = true;
        }

        private IEnumerator ArmAfter()
        {
            yield return new WaitForSeconds(ArmTime);
            foreach (var component in GetComponents<Collider>())
            {
                component.enabled = true;
            }
            armed = true;
        }

        public void Update()
        {
            if (Target != null && Target.IsDestroyed)
            {
                Target.LockedOff(this);
                Target = null;
            }

            if (Target == null)
            {
                var forward = transform.forward;

                var candidates = new List<CanBeLockedOnTo>();

                foreach (var targetable in SubManager.Instance.Targetable)
                {
                    if (targetable.transform.gameObject == Parent.gameObject)
                        continue;

                    var vectorTorpToTargetable = targetable.transform.position - transform.position;

                    RaycastHit hit;
                    Physics.Raycast(new Ray(transform.position, vectorTorpToTargetable), out hit);

                    var hitTargetable = hit.collider.gameObject.GetComponentInParent<CanBeLockedOnTo>();

                    if (hitTargetable == null || hitTargetable != targetable)
                        continue;

                    var distance = Vector3.Distance(transform.position, targetable.transform.position);
                    if (distance > HomingRange)
                        continue;

                    var isInCone = Vector3.Angle(forward, vectorTorpToTargetable.normalized) < HomingConeHalfAngle;

                    if (isInCone)
                        candidates.Add(targetable);
                }

                if (candidates.Count == 0)
                    return;

                var closest = candidates.OrderBy(x => Vector3.Distance(transform.position, x.transform.position)).First();
                Target = closest;
                Target.IsLockedOn(this);
            }
            else
            {
                var newRotation = Quaternion.RotateTowards(Quaternion.LookRotation(direction),
                    Quaternion.LookRotation(Target.transform.position - transform.position), HomingTurnRate * Time.deltaTime);
                direction = newRotation * Vector3.forward;
            }
        }

        public void FixedUpdate()
        {
            body.velocity = direction * speed;
            if (speed < MaxSpeed)
            {
                speed += Acceleration + Time.fixedDeltaTime;
            }

            transform.rotation = Quaternion.LookRotation(direction);
        }

        public void OnCollisionEnter(Collision collision)
        {
            Collided(collision);
        }

        public void OnCollisionStay(Collision collision)
        {
            Collided(collision);
        }

        private void Collided(Collision collision)
        {
            if (!armed)
                return;

            var targetable = collision.collider.GetComponentInParent<CanBeLockedOnTo>();
            if (targetable != null)
                targetable.HitByTorpedo(this);

            Explode();
        }

        private void Explode()
        {
            // get closest player to the torpedo or mine
            //   loop for each player,
            //   check the distance to the torpedo or mine
            // attenuate the sound of the explosion: 100% if direct hit to player, fade out to 10% if more than half arena away

            float closest = 99999;
            foreach (var player in SubManager.Instance.Subs)
            {
                var distance = Vector3.Distance(transform.position, player.transform.position);
                if (distance < closest)
                {
                    closest = distance;
                }
            }

            var attenuateDist = SoundManager.Instance.AttenuateDistance;

            var percentOfMax = Mathf.Clamp01(closest / attenuateDist);
            var volume = Mathf.Lerp(1, 0.1f, Mathf.Pow(percentOfMax, 2));
            
            SoundManager.PlaySound("explosion_far1", volume);

            Instantiate(ExplosionPrefab, transform.position, transform.rotation);
            Destroy(gameObject);
        }

        private void OnDestroy()
        {
            if (Target != null)
                Target.LockedOff(this);
        }

        public GameObject GameObject { get { return gameObject; } }
    }
}