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

	public void Explode()
	{
		Instantiate(ExplosionPrefab, transform.position, transform.rotation);
		Destroy(gameObject);
	}
}