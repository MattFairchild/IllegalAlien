using System;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;


public class chaseTarget : State, combat
{
    private NavMeshAgent agent;
    private GameObject bulletPrefab;
    private AudioSource audio;


    public void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.destination = transform.position;
        audio = GetComponent<AudioSource>();

        bulletPrefab = (GameObject)Resources.Load("/Prefabs/Bullets/BulletHostile");
    }


    public override void run()
    {
        shoot();
    }


    public void shoot()
    {
        audio.PlayOneShot(audio.clip);
        Vector3 dir = player.position - transform.position;
        BulletScript bs = (GameObject.Instantiate(bulletPrefab, transform.position + transform.forward, Quaternion.LookRotation(dir)) as GameObject).GetComponent<BulletScript>();
        bs.damage = 1.0f;
        bs.target = player;
    }
}