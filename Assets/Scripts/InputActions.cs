using UnityEngine;
using System.Collections;

public class InputActions : MonoBehaviour
{

    public GameObject turretPrefab;
    public InputCapture input;

    public float boostCooldown;

    private float maxSpeed = 7.0f;
    private Quaternion rot;
    private GameObject tempTurret;

    private bool bumperPressed = false, bumperReleased = false;


    // Update is called once per frame
    void FixedUpdate()
    {

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
        transform.position = transform.position + input.getSpeedRight() * maxSpeed * GameManager.getScreenRight() * Time.deltaTime;
        transform.position = transform.position + input.getSpeedUp() * maxSpeed * GameManager.getScreenUp() * Time.deltaTime;
        transform.position = new Vector3(transform.position.x, 0.0f, transform.position.z);


        //if the player is currently dragging a turret behind him, keep the position updated
        if (bumperPressed && !bumperReleased && tempTurret)
        {
            tempTurret.transform.position = this.transform.position - this.transform.forward * (transform.localScale.z / 2 + turretPrefab.transform.localScale.z / 2);
        }

        //rotate the palyer according to the angle. Mouse has only one rotation, Gamepad 2 seperate ones
        if (input.numberOfGamepads() > 0)
        {
            rot = Quaternion.AngleAxis(input.getFlightAngle(), GameManager.getScreenNormal());
        }
        else //keyboard&mouse
        {
            rot = Quaternion.AngleAxis(input.getShootAngle(), GameManager.getScreenNormal());
        }


        transform.rotation = rot;


        /*
            Placing Turret
         */

        //when first pressing place turret button, spawn turret attached to players rear
        if (input.placingTurret())
        {
            if (!bumperPressed)
            {
                //if radius should only be shown while having turret hovering, set variable
                if (!GameManager.getAlwaysShowRadius())
                {
                    GameManager.setShowRadius(true);
                }
                bumperPressed = true;
                bumperReleased = false;

                Vector3 spawnPos = this.transform.position - this.transform.forward * (transform.localScale.z / 2 + turretPrefab.transform.localScale.z / 2);
                tempTurret = (GameObject)GameObject.Instantiate(turretPrefab, spawnPos, this.transform.rotation);
                tempTurret.GetComponent<TurretScript>().setAffectedByGravity(false);
            }
        }
        //when releasing place turret button, let turret go
        if (!input.placingTurret())
        {
            if (bumperPressed && !bumperReleased)
            {
                //if radius should only be shown while having turret hovering, set variable
                if (!GameManager.getAlwaysShowRadius())
                {
                    GameManager.setShowRadius(false);
                }

                bumperPressed = false;
                bumperReleased = true;

                //check if there is a tempTurret. (possible error: turret hit planet while player was setting it)
                if (tempTurret)
                {
                    tempTurret.GetComponent<TurretScript>().SetVelocity(this.transform.forward * input.getSpeed() * maxSpeed);
                }

                boostCooldown = 0.5f;
            }
        }

    }
}
