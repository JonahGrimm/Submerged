using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateForce : MonoBehaviour
{
    Rigidbody rb;
    public bool activate;
    public float force;
    public Vector3 offSet;
    public float radius;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        if (activate)
        {
            activate = false;
            rb.AddExplosionForce(force, transform.position + offSet, radius);
        }
    }
}
