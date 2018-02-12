using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpSpawnPoint : MonoBehaviour
{

    private bool empty = true;
    private float timer;

    

    public int TimeToSpawn;
    public GameObject[] pickups;
    public GameObject ThisPickup;

	// Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (empty == true)
        {
            timer += Time.deltaTime;
            if (timer >= TimeToSpawn)
            {
                empty = false;
                ThisPickup = Instantiate(pickups[Random.Range(0, pickups.Length)], transform.position, Quaternion.identity);
            }
        }
        if (empty == false && ThisPickup == null)
        {
            empty = true;
        }
	}
}
