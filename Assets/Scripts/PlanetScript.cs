using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlanetScript : MonoBehaviour
{
    public Rigidbody rb;

    // Use this for initialization
    void Start()
    {
        rb.mass = transform.localScale.x;
    }
}
