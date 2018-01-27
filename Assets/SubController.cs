using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubController : MonoBehaviour {

    private bool kcF;
    private bool kcD;
    private bool kcRightArrow;
    private bool kcLeftArrow;
    private bool kcUpArrow;
    private bool kcDownArrow;

    private Rigidbody rb;    

    // Use this for initialization
    void Start () {
        rb = GetComponent<Rigidbody>();
	}

    void FixedUpdate()
    {
        Vector3 newPosition = rb.transform.position;

        rb.transform.position = newPosition;
        rb.AddForce(force);

        if (kcRightArrow)
        {
            kcRightArrow = false;
        }
        if (kcLeftArrow)
        {
            kcLeftArrow = false;
        }
        if (kcUpArrow)
        {
            kcUpArrow = false;
        }
        if (kcDownArrow)
        {
            kcDownArrow = false;
        }
        if (kcF) // increase forward force
        {
            kcF = false;
        }
        if (kcD) // decrease forward force
        {
            kcD = false;
        }
    }

    // Update is called once per frame
    void Update () {

        if (Input.GetKey(KeyCode.RightArrow))
        {
            kcRightArrow = true;
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            kcLeftArrow = true;
        }
        if (Input.GetKey(KeyCode.UpArrow))
        {
            kcUpArrow = true;
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            kcDownArrow = true;
        }
        if (Input.GetKeyDown(KeyCode.F)) // increase forward force
        {
            kcF = true;
        }
        if (Input.GetKeyDown(KeyCode.D)) // decrease forward force
        {
            kcD = true;
        }
    }
}
