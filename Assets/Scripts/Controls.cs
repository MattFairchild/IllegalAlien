using UnityEngine;
using System.Collections;

public class Controls : MonoBehaviour {

    public GameObject turretPrefab;

    public float maxSpeed = 5.5f;
    public float speed;
    private float angle;
    private Quaternion rot;
    private GameObject cam;
    private Vector3 screenUp, screenRight;

	// Use this for initialization
	void Start () {
        cam = GameObject.FindGameObjectWithTag("MainCamera");
        screenUp = cam.transform.up;
        screenRight = cam.transform.right;
	}
	
	// Update is called once per frame
	void FixedUpdate () {

        //rot = Quaternion.AngleAxis(-angle, cam.transform.forward);
        //transform.rotation = rot;

        //move position up/down + left/right on the screen
        float speedR = Input.GetAxis("HorizontalLeft") * maxSpeed;
        float speedU = Input.GetAxis("VerticalLeft") * maxSpeed;

        transform.position = transform.position + speedR * screenRight * Time.deltaTime;
        transform.position = transform.position + speedU * screenUp * Time.deltaTime;
        transform.position = new Vector3(transform.position.x, 0.0f, transform.position.z);

        speed = new Vector3(speedR, 0.0f, speedU).magnitude;

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
        }

        //right bmber
        if (Input.GetAxis("BumperR") > 0.1f)
        {
            Vector3 spawnPos = this.transform.forward * -0.5f;
            GameObject tempTurret = (GameObject)GameObject.Instantiate(turretPrefab, spawnPos, this.transform.rotation);
            tempTurret.GetComponent<TurretScript>().direction = this.transform.forward;
            tempTurret.GetComponent<TurretScript>().speed = speed;
        }

	}
}
