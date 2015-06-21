using System;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;


public abstract class State : MonoBehaviour
{
    protected Transform player, station;
    protected GameObject parent;
    protected NavMeshAgent agent;
    protected AudioSource audio;

    public void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        station = GameObject.FindGameObjectWithTag("SpaceStation").transform;
        parent = transform.root.gameObject;
        agent = parent.GetComponent<NavMeshAgent>();

        audio = parent.GetComponent<AudioSource>();
    }

    public abstract void run();
}
