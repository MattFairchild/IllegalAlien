using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class GunScript : MonoBehaviour {

	public AudioClip shotSound;
    public InputCapture input;
    public GameObject bulletPrefab;
    public float bulletCooldown;

	private AudioSource audio;

	void Start()
	{
		audio = GetComponent<AudioSource> ();
	}


	// Update is called once per frame
	void Update () {

        Quaternion rot;

        //if shooting on CD, then decrease cd with every update
        if (bulletCooldown > 0.0f)
        {
            bulletCooldown -= Time.deltaTime;
        }
        //depending on inout method different direction to face
        if (input.numberOfGamepads() > 0)
        {
            rot = Quaternion.AngleAxis(input.getShootAngle(), GameManager.getScreenNormal());
        }
        else //keyboard&mouse
        {
            rot = Quaternion.AngleAxis(input.getShootAngle(), GameManager.getScreenNormal());
        }

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
                float safetyDistance = transform.localScale.z + bulletPrefab.transform.localScale.y / 2;
                Vector3 spawnPos = this.transform.position + this.transform.forward * safetyDistance;
                GameObject.Instantiate(bulletPrefab, spawnPos, this.transform.rotation);
            }
        }
	}
}
