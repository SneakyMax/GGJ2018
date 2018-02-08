using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets;
using UnityEngine;

public class Mine : MonoBehaviour
{
	public float ArmTime = 5;

	public Sub Parent { get; set; }

	public GameObject ExplosionPrefab;

	private bool armed;
    private Rigidbody body;

    public float SeekAccel;

    public float MaxSpeed;

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

		var targetable = collision.collider.GetComponentInParent<TorpedoTargetable>();
		if (targetable != null)
			targetable.HitByMine(this);

		Explode();
	}

	private void Explode()
	{
		Instantiate(ExplosionPrefab, transform.position, transform.rotation);
		Destroy(gameObject);

        SoundManager.PlaySound("explosion_medium - far");
    }

    public void FixedUpdate()
    {
        if (armed == false)
            return;

        var closestPlayer =
            SubManager.Instance.Subs.OrderBy(x => Vector3.Distance(transform.position, x.transform.position))
                .FirstOrDefault();

        if (closestPlayer == null)
            return;

        var toClosestPlayer = (closestPlayer.transform.position - transform.position).normalized;

        if ( body.velocity.magnitude < MaxSpeed)
            body.AddForce(toClosestPlayer * SeekAccel, ForceMode.Acceleration);
    }
}