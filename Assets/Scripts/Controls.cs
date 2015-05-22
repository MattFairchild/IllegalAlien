using UnityEngine;
using System.Collections;

public class Controls : MonoBehaviour {

    public GameObject turretPrefab;
    public GameObject bulletPrefab;

    public float bulletCooldown, boostCooldown;

    private float speed;
    private float angle;
    private float maxSpeed = 7.0f;
    private Quaternion rot;
    private GameObject cam;
    private GameObject tempTurret;


    private Vector3 screenUp, screenRight;
    private bool bumperPressed = false, bumperReleased = false;

	// Use this for initialization
	void Start () {
        cam = GameObject.FindGameObjectWithTag("MainCamera");
        screenUp = cam.transform.up;
        screenRight = cam.transform.right;
	}
	
	// Update is called once per frame
	void FixedUpdate () {

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

        //move position up/down + left/right on the screen
        float speedR = Input.GetAxis("HorizontalLeft") * maxSpeed;
        float speedU = Input.GetAxis("VerticalLeft") * maxSpeed;
        speed = new Vector3(speedR, 0.0f, speedU).magnitude;

        transform.position = transform.position + speedR * screenRight * Time.deltaTime;
        transform.position = transform.position + speedU * screenUp * Time.deltaTime;
        transform.position = new Vector3(transform.position.x, 0.0f, transform.position.z);

        //if the player is currently dragging a turret behind him, keep the position updated
        if (bumperPressed && !bumperReleased)
        {
            tempTurret.transform.position = this.transform.position - this.transform.forward * (transform.localScale.z / 2 + turretPrefab.transform.localScale.z);
        }


        //rotate towards how the right analog stick is facing
        float y = Input.GetAxis("HorizontalRight");
        float x = Input.GetAxis("VerticalRight");


        if (x != 0.0f || y != 0.0f)
        {
            angle = (Mathf.Atan2(y, x) * Mathf.Rad2Deg) + 90.0f;
        }
        else
        {
            angle = 0.0f;
        }

        //transform.Rotate(cam.transform.forward, angle);
            rot = Quaternion.AngleAxis(angle, cam.transform.forward);
            transform.rotation = rot;

        // A Button
        if (Input.GetButton("Fire1"))
        {
        }

        //left bumper
        if (Input.GetAxis("BumperL") > 0.1f)
        {
            if (bulletCooldown <= 0.0f)
            {
                bulletCooldown = 0.2f;
                Vector3 spawnPos = this.transform.position + this.transform.forward * transform.localScale.z;
                GameObject.Instantiate(bulletPrefab, spawnPos, this.transform.rotation);
            }
        }

        //right bmber
        if (Input.GetAxis("BumperR") > 0.1f)
        {
            if (!bumperPressed)
            { 
                bumperPressed = true;
                bumperReleased = false;

                Vector3 spawnPos = this.transform.position - this.transform.forward*(transform.localScale.z/2 + turretPrefab.transform.localScale.z);
                tempTurret = (GameObject)GameObject.Instantiate(turretPrefab, spawnPos, this.transform.rotation);
                tempTurret.GetComponent<TurretScript>().setAffectedByGravity(false);            
            }
        }
        if (Input.GetAxis("BumperR") < 0.1f)
        {
            if (bumperPressed && !bumperReleased)
            {
                bumperPressed = false;
                bumperReleased = true;

                tempTurret.GetComponent<TurretScript>().SetVelocity(this.transform.forward * speed);
                boostCooldown = 0.5f;
            }
        }

	}
}
