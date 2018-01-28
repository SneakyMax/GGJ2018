using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets;
using UnityEngine;

public class Torpedo : MonoBehaviour
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

    private float speed;
    private Vector3 direction;
    public TorpedoTargetable target;
    private float spawnTime;
    private bool armed;

    public void Awake()
    {
        body = GetComponent<Rigidbody>();
    }

    public void Start()
    {
        speed = InitialSpeed + Parent.GetComponent<Rigidbody>().velocity.magnitude;
        direction = transform.forward;

        foreach (var component in GetComponents<Collider>())
        {
            component.enabled = false;
        }

        StartCoroutine(BlowUpAfterTime());
        StartCoroutine(ArmAfter());
        spawnTime = Time.time;
    }

    private IEnumerator BlowUpAfterTime()
    {
        yield return new WaitForSeconds(MaxLifetime);
        Explode();
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
        if (target == null)
        {
            var forward = transform.forward;

            var candidates = new List<TorpedoTargetable>();

            foreach (var targetable in SubManager.Instance.Targetable)
            {
                if (targetable.transform.gameObject == Parent.gameObject)
                    continue;

                var vectorTorpToTargetable = targetable.transform.position - transform.position;

                RaycastHit hit;
                Physics.Raycast(new Ray(transform.position, vectorTorpToTargetable), out hit);

                var hitTargetable = hit.collider.gameObject.GetComponentInParent<TorpedoTargetable>();

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
            target = closest;
            Debug.Log(String.Format("Torpedo targeting {0}", target.gameObject.name));
        }
        else
        {
            var newRotation = Quaternion.RotateTowards(Quaternion.LookRotation(direction),
                Quaternion.LookRotation(target.transform.position - transform.position), HomingTurnRate * Time.deltaTime);
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
        if (!armed)
            return;

        var targetable = collision.collider.GetComponentInParent<TorpedoTargetable>();
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

        SoundManager.PlaySound("explosion_far1");

        Instantiate(ExplosionPrefab, transform.position, transform.rotation);
        Destroy(gameObject);
    }
}