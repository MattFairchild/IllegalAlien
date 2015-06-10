using UnityEngine;
using System.Collections;

public class InputActions : MonoBehaviour
{

    public GameObject turretPrefab;
    public InputCapture input;

    public float boostCooldown;
	public float boost;
    
    public float maxSpeed;
    private float currentMaxSpeed;
	private Quaternion rot; 
	private Quaternion defaultRot;
    private GameObject tempTurret;

    private bool bumperPressed = false, bumperReleased = false;


	void Start()
	{
		boost = 1.0f; 
	}

    // Update is called once per frame
    void FixedUpdate()
    {
        /*
            Placing Turret
         */

		if (input.leftTrigger ()) 
		{
			if(boost > 0.0f)
			{
				boost -= 0.25f*Time.deltaTime;
				currentMaxSpeed = 2.0f*maxSpeed;
			}
		}
		else 
		{
			if(GameManager.getBoostTime() < 1.0f)
			{
				boost += 0.1f*Time.deltaTime;
			}
		}

		if (boost < 0.0f)
			boost = 0.0f;
		if (boost > 1.0f)
			boost = 1.0f;


		GameManager.setBoost (boost);


		if (!GameManager.mixedControls ()) 
		{
			placingMovement();
		}

        //when first pressing place turret button, spawn turret attached to players rear
        if (input.placingTurret())
        {
			if (GameManager.mixedControls ()) 
			{	
				placingMovement();
			}

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
        else
        {
			if (GameManager.mixedControls ()) 
			{	
				normalMovement();
			}

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
						trt.SetVelocity(this.transform.forward * input.getSpeed() * currentMaxSpeed);
					}
                }

                boostCooldown = 0.5f;
            }
        }

		if (input.rightAnalogMoving ()) 
		{
			transform.rotation = rot;
		} 
		else 
		{
			transform.rotation = defaultRot;
		}
    }

	void placingMovement()
	{
		//if player has short boost after setting turret, decrease the bosst time every update
		if (!input.leftTrigger ()) 
		{
			if (boostCooldown > 0.0f)
			{
				currentMaxSpeed = 1.5f * maxSpeed;
				boostCooldown -= Time.deltaTime;
			}
			else
			{
				currentMaxSpeed = maxSpeed;
			}
		}
		
		//adjust position according to speed etc.
		transform.position = transform.position + input.getSpeedRight() * currentMaxSpeed * GameManager.getScreenRight() * Time.deltaTime;
		transform.position = transform.position + input.getSpeedUp() * currentMaxSpeed * GameManager.getScreenUp() * Time.deltaTime;
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

		defaultRot = rot;
	}


	void normalMovement()
	{	
		//if player has short boost after setting turret, decrease the bosst time every update
		if (!input.leftTrigger ()) 
		{
			if (boostCooldown > 0.0f)
			{
				currentMaxSpeed = 1.5f * maxSpeed;
				boostCooldown -= Time.deltaTime;
			}
			else
			{
				currentMaxSpeed = maxSpeed;
			}
		}


		//adjust position according to speed etc.
		transform.position = transform.position + input.getSpeedRight() * currentMaxSpeed * GameManager.getScreenRight() * Time.deltaTime;
		transform.position = transform.position + input.getSpeedUp() * currentMaxSpeed * GameManager.getScreenUp() * Time.deltaTime;
		transform.position = new Vector3(transform.position.x, 0.0f, transform.position.z);
		
		//rotate the player according to the angle of the right analog stick if he is currently not placing a turret
		if (input.numberOfGamepads() > 0)
		{
			rot = Quaternion.AngleAxis(input.getShootAngle(), GameManager.getScreenNormal());
		}
		else //keyboard&mouse
		{
			rot = Quaternion.AngleAxis(input.getShootAngle(), GameManager.getScreenNormal());
		}
	}
}
