using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlanetScript : MonoBehaviour
{

    public float mass;


    // Use this for initialization
    void Start()
    {
        mass = transform.localScale.x;
    }


    void FixedUpdate()
    {

    }


}
