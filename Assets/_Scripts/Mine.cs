using System.Collections;
using System.Linq;
using UnityEngine;

namespace Depth
{
    public class Mine : MonoBehaviour, ICanLockOn
    {
        public Sub Parent { get; set; }

        public GameObject ExplosionPrefab;
        public float SeekAccel;
        public float MaxSpeed;
        public float ArmTime = 5;

        public float MinRangeToShowTargeted = 15;

        private CanBeLockedOnTo target;

        private bool armed;
        private Rigidbody body;

        public void Awake()
        {
            body = GetComponent<Rigidbody>();
        }

        public void Start()
        {
            StartCoroutine(ArmAfter());
        }

        private IEnumerator ArmAfter()
        {
            yield return new WaitForSeconds(ArmTime);
            armed = true;
        }
		
        public void OnCollisionEnter(Collision collision)
        {
            if (!armed)
                return;

            var targetable = collision.collider.GetComponentInParent<CanBeLockedOnTo>();
            if (targetable != null)
                targetable.HitByMine(this);

            Explode();
        }

        private void Explode()
        {
            Instantiate(ExplosionPrefab, transform.position, transform.rotation);
            Destroy(gameObject);

            SoundManager.PlaySound("explosion_medium-far");
        }

        public void FixedUpdate()
        {
            if (!armed)
                return;

            if (target != null)
            {
                var toClosestPlayer = (target.transform.position - transform.position).normalized;

                if (body.velocity.magnitude < MaxSpeed)
                    body.AddForce(toClosestPlayer * SeekAccel, ForceMode.Acceleration);
            }
        }

        public void Update()
        {
            if (target != null && target.IsDestroyed)
            {
                target.LockedOff(this);
                target = null;
            }

            if (target != null && Vector3.Distance(target.transform.position, transform.position) > MinRangeToShowTargeted)
            {
                target.LockedOff(this);
            }

            if (!armed)
                return;

            var previousTarget = target;
            target = SubManager.Instance.Targetable.OrderBy(x => Vector3.Distance(transform.position, x.transform.position))
                .FirstOrDefault();

            if (previousTarget != null && target != previousTarget)
            {
                previousTarget.LockedOff(this);
            }

            if (target != null && Vector3.Distance(target.transform.position, transform.position) < MinRangeToShowTargeted)
            {
                target.IsLockedOn(this);
            }
        }

        public void OnDestroy()
        {
            if (target != null)
            {
                target.LockedOff(this);
            }
        }

        public GameObject GameObject { get { return gameObject; } }
    }
}