using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets;
using UnityEngine;

public class Mine : MonoBehaviour
{
	public Sub Parent { get; set; }

	public GameObject ExplosionPrefab;

	public void OnCollisionEnter(Collision collision)
	{
		var targetable = collision.collider.GetComponentInParent<TorpedoTargetable>();
		if (targetable != null)
			targetable.HitByTorpedo(this);
		Explode();
	}

	public void Explode()
	{
		Instantiate(ExplosionPrefab, transform.position, transform.rotation);
		Destroy(gameObject);
	}
}