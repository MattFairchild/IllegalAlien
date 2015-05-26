using UnityEngine;
using System.Collections;

public class GunScript : MonoBehaviour {

    public GameManager gm;

    public InputCapture input;
    public GameObject bulletPrefab;
    public float bulletCooldown;

    void Start()
    {
        gm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
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
            rot = Quaternion.AngleAxis(input.getShootAngle(), gm.screenNormal);
        }
        else //keyboard&mouse
        {
            rot = Quaternion.AngleAxis(input.getShootAngle(), gm.screenNormal);
        }

        this.transform.rotation = rot;


        /*
        Shooting
        */

        if (input.isShooting())
        {
            if (bulletCooldown <= 0.0f)
            {
                bulletCooldown = 0.25f;
                float safetyDistance = transform.localScale.z + bulletPrefab.transform.localScale.y / 2;
                Vector3 spawnPos = this.transform.position + this.transform.forward * safetyDistance;
                GameObject.Instantiate(bulletPrefab, spawnPos, this.transform.rotation);
            }
        }
	}
}
