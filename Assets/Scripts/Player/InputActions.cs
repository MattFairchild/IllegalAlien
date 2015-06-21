using UnityEngine;
using System.Collections;

public class InputActions : MonoBehaviour
{
    public GameObject turretPrefabLvl1;
	public GameObject turretPrefabLvl2;
	public GameObject turretPrefabLvl3;
	public GameObject turretPrefabLvl4;
	public GameObject warningParticlePrefab;
	public InputCapture input;
	[SerializeField]protected new Rigidbody rigidbody;

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

	protected void TryToSpawnTurret () {
		int lvl = GameManager.BuyMaxCurAvailableTower();
		if(lvl > 0){
			SpawnTurret(lvl);
		}
		else{
			ShowWarning();
		}
	}

	protected void SpawnTurret (int lvl = 1) {
		GameObject turretPrefab = 1 == lvl ? turretPrefabLvl1 : 2 == lvl ? turretPrefabLvl2 : 3 == lvl ? turretPrefabLvl3 : turretPrefabLvl4;
		Vector3 spawnPos = this.transform.position - this.transform.forward * (transform.localScale.z / 2 + turretPrefabLvl1.transform.localScale.z / 2);
		tempTurret = (GameObject)Instantiate(turretPrefab, spawnPos, this.transform.rotation);
		tempTurret.GetComponent<TurretScript>().setAffectedByGravity(false);
	}

	protected void ReleaseTurret () {
		//check if there is a tempTurret. (possible error: turret hit planet while player was setting it)
		if (tempTurret){
			TurretScript trt = tempTurret.GetComponent<TurretScript>();
			if(trt){
				trt.SetVelocity(this.transform.forward * input.getSpeed() * maxSpeed);
			}
			tempTurret = null;
		}
	}

	protected void ShowWarning () {
		Vector3 spawnPos = this.transform.position - this.transform.forward * (transform.localScale.z / 2 + warningParticlePrefab.transform.localScale.z / 2) + Vector3.up;
		(Instantiate(warningParticlePrefab, spawnPos, Quaternion.identity) as GameObject).transform.SetParent(this.transform);
	}

	protected void MovePlayer () {
		//adjust position according to speed etc.
		//transform.position = transform.position + input.getSpeedRight() * currentMaxSpeed * GameManager.getScreenRight() * Time.deltaTime;
		//transform.position = transform.position + input.getSpeedUp() * currentMaxSpeed * GameManager.getScreenUp() * Time.deltaTime;
		//transform.position = new Vector3(transform.position.x, 0.0f, transform.position.z);

		Vector3 movement = (input.getSpeedUp() * GameManager.getScreenUp() + input.getSpeedRight() * GameManager.getScreenRight());
		movement *= currentMaxSpeed * Time.deltaTime;
		Vector3 newPosition = rigidbody.position + movement;
		newPosition.y = 0;
		rigidbody.MovePosition(newPosition);

		//ideas: 
		//1. transform.Translate? even better: rigidbody.MovePosition(); (physics detection then!)
		//2. input up and right --> direction; what's the maximum possibl speed/vector magnitude for that direction? --> divide current speed by max --> "directional normalizing"
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
                    GameManager.showRadius = true;
                }
                bumperPressed = true;
                bumperReleased = false;

				TryToSpawnTurret();
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
                    GameManager.showRadius = false;
                }

                bumperPressed = false;
                bumperReleased = true;

				ReleaseTurret();
            }
        }


        //if the player is currently dragging a turret behind him, keep the position updated
        if (bumperPressed && !bumperReleased && tempTurret)
        {
            tempTurret.transform.position = this.transform.position - this.transform.forward * (transform.localScale.z / 2 + turretPrefabLvl1.transform.localScale.z / 2);
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
        
		MovePlayer();

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
                    GameManager.showRadius = true;
                }
                bumperPressed = true;
                bumperReleased = false;

				TryToSpawnTurret();
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
                    GameManager.showRadius = false;
                }

                bumperPressed = false;
                bumperReleased = true;

				ReleaseTurret();
            }
        }


        //if the player is currently dragging a turret behind him, keep the position updated
        if (bumperPressed && !bumperReleased && tempTurret)
        {
            tempTurret.transform.position = this.transform.position - this.transform.forward * (transform.localScale.z / 2 + turretPrefabLvl1.transform.localScale.z / 2);
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
			MovePlayer();
        }
    }

}
