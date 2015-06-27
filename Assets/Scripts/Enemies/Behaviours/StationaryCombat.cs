using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;


public class StationaryCombat : State, ICombat
{
    protected GameObject bulletPrefab;

    void Awake()
    {
        bulletPrefab = (GameObject)Resources.Load("/Prefabs/Bullets/BulletHostile");
    }

    public override void run()
    {
        Debug.Log("STATIONARY");

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