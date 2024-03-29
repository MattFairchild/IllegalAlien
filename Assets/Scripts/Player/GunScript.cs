﻿using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class GunScript : MonoBehaviour
{

    public AudioClip shotSound;
    public InputCapture input;
    public GameObject bulletPrefab;
    public Transform parent;
    public float bulletCooldown;
    public float projectileOffset;

    private new AudioSource audio;

    void Start()
    {
        audio = GetComponent<AudioSource>();
        //parent = GameObject.FindGameObjectWithTag ("Player").transform;
        parent = transform.parent;
    }


    // Update is called once per frame
    void Update()
    {

        Quaternion rot;

        //if shooting on CD, then decrease cd with every update
        if (bulletCooldown > 0.0f)
        {
            bulletCooldown -= Time.deltaTime;
        }
        //depending on inout method different direction to face
        if (input.numberOfGamepads() > 0)
        {
            if (GameManager.mixedControls())
            {
                rot = parent.rotation;
            }
            else
            {
                rot = Quaternion.AngleAxis(input.getShootAngle(), GameManager.getScreenNormal());
            }
        }
        else //keyboard&mouse
        {
            rot = Quaternion.AngleAxis(input.getShootAngle(), GameManager.getScreenNormal());
        }

        rot *= Quaternion.Euler(90, 0, 0);

        this.transform.rotation = rot;


        /*
        Shooting
        */
        if ((input.isShooting() && !GameManager.getAutoShoot()) || (input.rightAnalogMoving() && GameManager.getAutoShoot()))
        {
            if (bulletCooldown <= 0.0f)
            {
                audio.PlayOneShot(shotSound, 0.1f);
                bulletCooldown = 0.25f;
                float safetyDistance = projectileOffset + bulletPrefab.transform.localScale.y / 2;
                Vector3 spawnPos = this.transform.position + this.transform.up * safetyDistance;
                GameObject.Instantiate(bulletPrefab, spawnPos, this.transform.rotation);
            }
        }
    }
}
