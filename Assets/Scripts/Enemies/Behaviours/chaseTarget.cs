﻿using System;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;


public class ChaseTarget : State, ICombat
{
    protected GameObject bulletPrefab;

    void Awake()
    {
        bulletPrefab = (GameObject)Resources.Load("/Prefabs/Bullets/BulletHostile");
    }

    public override void run()
    {
        agent.destination = player.transform.position;

        /*
        CAREFUL: shooting is weird at the moment + enemies shoot through other script atm (related?)
        */
        //shoot();
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