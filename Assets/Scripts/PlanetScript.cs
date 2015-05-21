using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlanetScript : MonoBehaviour
{

    public float mass;
    public Rigidbody rb;
    public float bla;
    public float distance;

    // Use this for initialization
    void Start()
    {
        //mass = transform.localScale.x;
        rb.mass = transform.localScale.x;
        //Orbital speed
        //TurretScript ts = GetComponent<TurretScript>();
        //float g = ts.GetGravitationalConstant();
        //bla = Mathf.Sqrt(g * rb.mass / distance);
    }


    void FixedUpdate()
    {

    }


}
