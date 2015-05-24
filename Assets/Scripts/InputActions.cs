using UnityEngine;
using System.Collections;

public class InputActions : MonoBehaviour {

    public GameObject turretPrefab;
    public GameObject bulletPrefab;
    public InputCapture input;

    public float bulletCooldown, boostCooldown;

    private float maxSpeed = 7.0f;
    private Quaternion rot;
    private GameObject cam;
    private GameObject tempTurret;


    private Vector3 screenUp, screenRight;
    private bool bumperPressed = false, bumperReleased = false;

    // Use this for initialization
    void Start()
    {
        cam = GameObject.FindGameObjectWithTag("MainCamera");
        screenUp = cam.transform.up;
        screenRight = cam.transform.right;
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        Debug.Log(maxSpeed);

        //if shooting on CD, then decrease cd with every update
        if (bulletCooldown > 0.0f)
        {
            bulletCooldown -= Time.deltaTime;
        }

        //if player has short boost after setting turret, decrease the bosst time every update
        if (boostCooldown > 0.0f)
        {
            maxSpeed = 10.0f;
            boostCooldown -= Time.deltaTime;
        }
        else
        {
            maxSpeed = 7.0f;
        }

        //adjust position according to speed etc.
        transform.position = transform.position + input.getSpeedRight() * maxSpeed * screenRight * Time.deltaTime;
        transform.position = transform.position + input.getSpeedUp() * maxSpeed * screenUp * Time.deltaTime;
        transform.position = new Vector3(transform.position.x, 0.0f, transform.position.z);


        //if the player is currently dragging a turret behind him, keep the position updated
        if (bumperPressed && !bumperReleased)
        {
            tempTurret.transform.position = this.transform.position - this.transform.forward * (transform.localScale.z / 2 + turretPrefab.transform.localScale.z);
        }

        //rotate the palyer according to the angle
        rot = Quaternion.AngleAxis(input.getAngle(), cam.transform.forward);
        transform.rotation = rot;



        /*
            Shooting
         */

        if (input.isShooting())
        {
            if (bulletCooldown <= 0.0f)
            {
                bulletCooldown = 0.2f;
                Vector3 spawnPos = this.transform.position + this.transform.forward * transform.localScale.z;
                GameObject.Instantiate(bulletPrefab, spawnPos, this.transform.rotation);
            }
        }




        /*
            Placing Turret
         */

        //when first pressing place turret button, spawn turret attached to players rear
        if (input.placingTurret())
        {
            if (!bumperPressed)
            {
                bumperPressed = true;
                bumperReleased = false;

                Vector3 spawnPos = this.transform.position - this.transform.forward * (transform.localScale.z / 2 + turretPrefab.transform.localScale.z);
                tempTurret = (GameObject)GameObject.Instantiate(turretPrefab, spawnPos, this.transform.rotation);
                tempTurret.GetComponent<TurretScript>().setAffectedByGravity(false);
            }
        }
        //when releasing place turret button, let turret go
        if (!input.placingTurret())
        {
            if (bumperPressed && !bumperReleased)
            {
                bumperPressed = false;
                bumperReleased = true;

                tempTurret.GetComponent<TurretScript>().SetVelocity(this.transform.forward * input.getSpeed() * maxSpeed);
                boostCooldown = 0.5f;
            }
        }

    }
}
