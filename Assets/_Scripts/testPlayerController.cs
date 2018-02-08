using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testPlayerController : MonoBehaviour {

    //[Range(0, 10)]
    public float throttle;
    public Rigidbody rb;
    public float acceleration;
    public float turnAcceleration;

	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {

        throttle += Input.GetAxisRaw("Horizontal P1") * Time.deltaTime;
//        if (Input.GetKey(KeyCode.W))
//        {
//            throttle += .1f;
//        }

        rb.AddForce(transform.forward * throttle);
        Debug.Log(Input.GetAxisRaw("Horizontal P1"));
	}
}
