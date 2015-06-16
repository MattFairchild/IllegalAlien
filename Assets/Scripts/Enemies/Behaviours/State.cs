using System;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;


public abstract class State : MonoBehaviour {
    protected Transform player, station;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        station = GameObject.FindGameObjectWithTag("SpaceStation").transform;
    }

    public abstract void run();
}
