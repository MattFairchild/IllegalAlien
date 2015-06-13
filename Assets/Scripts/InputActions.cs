using UnityEngine;
using System.Collections;

public class InputActions : MonoBehaviour
{
    public GameObject turretPrefab;
    public InputCapture input;

    private float boostCooldown;
    
    public float turretSpeed = 5;
    private float currentMaxSpeed, maxSpeed = 5.0f;
	private Quaternion rot;
    private GameObject tempTurret;

    private bool bumperPressed = false, bumperReleased = false;

    void Update()
    {
        if (GameManager.mixedControls())
        {
            optionalMovement();
        }
        else
        {
            normalMovement();
        }
    }

    // Update is called once per frame
    void normalMovement()
    {
        //when first pressing place turret button, spawn turret attached to players rear
        if (input.placingTurret())
        {
            currentMaxSpeed = maxSpeed;

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
                tempTurret = (GameObject)Instantiate(turretPrefab, spawnPos, this.transform.rotation);
                tempTurret.GetComponent<TurretScript>().setAffectedByGravity(false);
            }
        }
        //when releasing place turret button, let turret go
        else
        {
            currentMaxSpeed = maxSpeed * 2.0f;

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
					TurretScript trt = tempTurret.GetComponent<TurretScript>();
					if(trt){
						trt.SetVelocity(this.transform.forward * input.getSpeed() * maxSpeed);
					}
                }
            }
        }


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
        

        //adjust position according to speed etc.
        transform.position = transform.position + input.getSpeedRight() * currentMaxSpeed * GameManager.getScreenRight() * Time.deltaTime;
        transform.position = transform.position + input.getSpeedUp() * currentMaxSpeed * GameManager.getScreenUp() * Time.deltaTime;
        transform.position = new Vector3(transform.position.x, 0.0f, transform.position.z);

        transform.rotation = rot;
    }



    public void optionalMovement()
    {
        //when first pressing place turret button, spawn turret attached to players rear
        if (input.placingTurret())
        {
            currentMaxSpeed = maxSpeed;

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
                tempTurret = (GameObject)Instantiate(turretPrefab, spawnPos, this.transform.rotation);
                tempTurret.GetComponent<TurretScript>().setAffectedByGravity(false);
            }
        }
        //when releasing place turret button, let turret go
        else
        {
            currentMaxSpeed = maxSpeed * 2.0f;

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
                    TurretScript trt = tempTurret.GetComponent<TurretScript>();
                    if (trt)
                    {
                        trt.SetVelocity(this.transform.forward * input.getSpeed() * maxSpeed);
                    }
                }
            }
        }


        //if the player is currently dragging a turret behind him, keep the position updated
        if (bumperPressed && !bumperReleased && tempTurret)
        {
            tempTurret.transform.position = this.transform.position - this.transform.forward * (transform.localScale.z / 2 + turretPrefab.transform.localScale.z / 2);
        }

        //rotate the palyer according to the angle. Mouse has only one rotation, Gamepad 2 seperate ones
        if (input.numberOfGamepads() > 0)
        {
            rot = Quaternion.AngleAxis(input.getShootAngle(), GameManager.getScreenNormal());
        }
        else //keyboard&mouse
        {
            rot = Quaternion.AngleAxis(input.getShootAngle(), GameManager.getScreenNormal());
        }


        transform.rotation = rot;


        if (bumperPressed && !bumperReleased)
        {
            //adjust position according to speed etc.
            transform.position = transform.position + (transform.forward * Mathf.Abs(input.getSpeedUp()) * currentMaxSpeed * Time.deltaTime);
            transform.position = new Vector3(transform.position.x, 0.0f, transform.position.z);
        }
        else
        {
            //adjust position according to speed etc.
            transform.position = transform.position + input.getSpeedRight() * currentMaxSpeed * GameManager.getScreenRight() * Time.deltaTime;
            transform.position = transform.position + input.getSpeedUp() * currentMaxSpeed * GameManager.getScreenUp() * Time.deltaTime;
            transform.position = new Vector3(transform.position.x, 0.0f, transform.position.z);
        }
    }

}
