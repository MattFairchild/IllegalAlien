using UnityEngine;
using System.Collections;
using System.Linq.Expressions;

public class InputActions : MonoBehaviour
{
    public GameObject turretPrefabLvl1;
    public GameObject turretPrefabLvl2;
    public GameObject turretPrefabLvl3;
    public GameObject turretPrefabLvl4;
    public GameObject warningParticlePrefab;
    public InputCapture input;
    [SerializeField]
    protected new Rigidbody rigidbody;

    private float boostCooldown;

    public float turretSpeed = 5;
    public float currentMaxSpeed, maxSpeed = 5.0f;
    private Quaternion rot;
    private GameObject tempTurret;
    private int turretLvl;

    private Transform[] centerTurrets;
    private int previousTurretLevel;

    private bool turretLevelChanged;
    private const float spawnCountdownStartValue = 1f;
    private Vector3 spawnStartScale;
    private float currentCountdownValue;
    private Material normalTurretMaterial;
    private Material spawnTurretMaterial;

    private bool triggerPressed = false, triggerReleased = false;
    private bool boost = false;

    private ParticleSystem[] thrustParticleSystem;

    void Start()
    {
        thrustParticleSystem = new ParticleSystem[2];
        Transform thrustParent = transform.FindChild("Thrust");
        thrustParticleSystem[0] = thrustParent.FindChild("Thrust Left").GetComponent<ParticleSystem>();
        thrustParticleSystem[1] = thrustParent.FindChild("Thrust Right").GetComponent<ParticleSystem>();

        Transform centerTurret = transform.FindChild("CenterTurret");
        normalTurretMaterial = centerTurret.GetComponent<MeshRenderer>().materials[0];
        spawnTurretMaterial = centerTurret.GetComponent<MeshRenderer>().materials[1];

        centerTurrets = new Transform[centerTurret.childCount];
        for (int i = 0; i < centerTurrets.Length; i++)
        {
            centerTurrets[i] = centerTurret.FindChild("Level" + (i + 1));
        }
        spawnStartScale = Vector3.one * 2;
    }


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

        foreach (ParticleSystem particleSystem in thrustParticleSystem)
        {
            particleSystem.startSpeed = (input.getSpeed() * currentMaxSpeed) / 2f;
            particleSystem.emissionRate = (input.getSpeed() * currentMaxSpeed) * 4;
        }


        turretLvl = GameManager.maxCurAvailableTowerLevel;

        if (turretLvl > previousTurretLevel)
            turretLevelChanged = true;

        int cappedturretLvl = turretLvl > 4 ? 4 : turretLvl;

        if (turretLevelChanged && currentCountdownValue > 0)
        {
            for (int i = 0; i < cappedturretLvl; i++)
            {
                centerTurrets[i].GetComponent<MeshRenderer>().material = spawnTurretMaterial;
                centerTurrets[i].GetComponent<MeshRenderer>().enabled = true;
                centerTurrets[i].transform.localScale = Vector3.Lerp(spawnStartScale, Vector3.one, (spawnCountdownStartValue - currentCountdownValue) * 3);
            }

            currentCountdownValue -= Time.deltaTime;
        }
        else
        {
            for (int i = 0; i < cappedturretLvl; i++)
            {
                centerTurrets[i].GetComponent<MeshRenderer>().material = normalTurretMaterial;
            }

            currentCountdownValue = spawnCountdownStartValue;
            turretLevelChanged = false;
        }

        spawnTurretMaterial.mainTextureOffset = new Vector2(0, Time.time);
        previousTurretLevel = turretLvl;
    }

    protected void TryToSpawnTurret()
    {
        turretLvl = GameManager.BuyMaxCurAvailableTower();
        if (turretLvl > 0)
        {
            SpawnTurret(turretLvl);
            for (int i = 0; i < 4; i++)
            {
                centerTurrets[i].GetComponent<MeshRenderer>().enabled = false;
            }
        }
        else
        {
            ShowWarning();
        }
    }

    protected void SpawnTurret(int lvl = 1)
    {
        GameObject turretPrefab = 1 == lvl ? turretPrefabLvl1 : 2 == lvl ? turretPrefabLvl2 : 3 == lvl ? turretPrefabLvl3 : turretPrefabLvl4;
        Vector3 spawnPos = this.transform.position - this.transform.forward * (transform.localScale.z / 2 + turretPrefabLvl1.transform.localScale.z / 2);
        tempTurret = (GameObject)Instantiate(turretPrefab, spawnPos, this.transform.rotation);
        tempTurret.GetComponent<TurretScript>().setAffectedByGravity(false);
    }

    protected void ReleaseTurret()
    {
        //check if there is a tempTurret. (possible error: turret hit planet while player was setting it)
        if (tempTurret)
        {
            TurretScript trt = tempTurret.GetComponent<TurretScript>();
            if (trt)
            {
                //trt.SetVelocity(this.transform.forward * input.getSpeed() * maxSpeed);
                trt.SetVelocity(this.transform.forward * input.getSpeedNormalizedLength() * maxSpeed);
            }
            tempTurret = null;
        }
    }

    protected void ShowWarning()
    {
        Vector3 spawnPos = this.transform.position - this.transform.forward * (transform.localScale.z / 2 + warningParticlePrefab.transform.localScale.z / 2) + Vector3.up;
        (Instantiate(warningParticlePrefab, spawnPos, Quaternion.identity) as GameObject).transform.SetParent(this.transform);
    }

    protected void MovePlayer()
    {
        /*
        //version 1: adjust position according to speed etc.
        transform.position = transform.position + input.getSpeedRight() * currentMaxSpeed * GameManager.getScreenRight() * Time.deltaTime;
        transform.position = transform.position + input.getSpeedUp() * currentMaxSpeed * GameManager.getScreenUp() * Time.deltaTime;
        transform.position = new Vector3(transform.position.x, 0.0f, transform.position.z);
        */

        /*
        //version 2: use rigidbody.MovePosition()
        Vector3 movement = (input.getSpeedUp() * GameManager.getScreenUp() + input.getSpeedRight() * GameManager.getScreenRight());
        movement *= currentMaxSpeed * Time.deltaTime;
        Vector3 newPosition = rigidbody.position + movement;
        newPosition.y = 0;
        rigidbody.MovePosition(newPosition);
        */

        //version 3: "normalize" speed independent of movement direction
        //idea: input up and right --> direction; what's the maximum possibl speed/vector magnitude for that direction? --> divide current speed by max --> "directional normalizing"
        //Vector3 movement = new Vector3(input.getSpeedRight(), 0.0f, input.getSpeedUp());
        /*Vector3 movement = (input.getSpeedUp() * GameManager.getScreenUp() + input.getSpeedRight() * GameManager.getScreenRight());
        if(movement != Vector3.zero){
            Vector3 dir = movement.normalized;
            int idx = Mathf.Abs(movement[0]) > Mathf.Abs(movement[2]) ? 0 : 2;
            Vector3 maxMovementInDir = movement * (1.0f / (Mathf.Abs(movement[idx])));
            float normalizedSpeed = movement.magnitude / maxMovementInDir.magnitude;
            Vector3 normalizedInput = dir * normalizedSpeed;
            Vector3 newPos = rigidbody.position + normalizedInput * currentMaxSpeed * Time.deltaTime;
            newPos.y = 0;
            rigidbody.MovePosition(newPos);
        }*/

        //version 4: moved to input capture and greatly simplified
        Vector3 newPos = rigidbody.position + input.getSpeedNormalized() * currentMaxSpeed * Time.deltaTime;
        newPos.y = 0;
        rigidbody.MovePosition(newPos);
    }

	protected void RotatePlayer (Quaternion rot) {
		//transform.rotation = rot;
		//rigidbody.MoveRotation(rot);
		rigidbody.MoveRotation(Quaternion.Slerp(rigidbody.rotation, rot, 5*Time.deltaTime));
	}


    // Update is called once per frame
    void normalMovement()
    {
        //when first pressing place turret button, spawn turret attached to players rear
        if (input.placingTurret())
        {
            currentMaxSpeed = maxSpeed;

            if (!triggerPressed)
            {
                //if radius should only be shown while having turret hovering, set variable
                if (!GameManager.getAlwaysShowRadius())
                {
                    GameManager.showRadius = false;
                }
                triggerPressed = true;
                triggerReleased = false;

                TryToSpawnTurret();
            }
        }
        //when releasing place turret button, let turret go
        else
        {
            //is the player currently boosting, and is it still possible if he is?
            if (input.leftBumper() && GameManager.boostTimer > 0.0f)
            {
                if (GameManager.boostTimer >= 1.0f && !boost)
                {
                    boost = true;
                }
            }
            else if (!input.leftBumper() && boost)
            {
                boost = false;
            }


            //according to if the player is boosting or not set the speed
            if (boost)
            {
                currentMaxSpeed = maxSpeed * 6.0f;
                GameManager.boostTimer -= Time.deltaTime;

                if (GameManager.boostTimer <= 0.0f)
                {
                    boost = false;
                    GameManager.boostTimer = 0.0f;
                }
            }
            else
            {
                currentMaxSpeed = maxSpeed * 2.0f;

                if (GameManager.boostTimer < 1.0f)
                {
                    GameManager.boostTimer += Time.deltaTime * 0.2f;
                }
            }


            if (triggerPressed && !triggerReleased)
            {
                //if radius should only be shown while having turret hovering, set variable
                if (!GameManager.getAlwaysShowRadius())
                {
                    GameManager.showRadius = false;
                }

                triggerPressed = false;
                triggerReleased = true;

                ReleaseTurret();
            }
        }


        //if the player is currently dragging a turret behind him, keep the position updated
        if (triggerPressed && !triggerReleased && tempTurret)
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

		RotatePlayer(rot);
    }



    public void optionalMovement()
    {
        //when first pressing place turret button, spawn turret attached to players rear
        if (input.placingTurret())
        {
            currentMaxSpeed = maxSpeed;

            if (!triggerPressed)
            {
                //if radius should only be shown while having turret hovering, set variable
                if (!GameManager.getAlwaysShowRadius())
                {
                    GameManager.showRadius = false;
                }
                triggerPressed = true;
                triggerReleased = false;

                TryToSpawnTurret();
            }
        }
        //when releasing place turret button, let turret go
        else
        {
            currentMaxSpeed = maxSpeed * 2.0f;

            if (triggerPressed && !triggerReleased)
            {
                //if radius should only be shown while having turret hovering, set variable
                if (!GameManager.getAlwaysShowRadius())
                {
                    GameManager.showRadius = false;
                }

                triggerPressed = false;
                triggerReleased = true;

                ReleaseTurret();
            }
        }


        //if the player is currently dragging a turret behind him, keep the position updated
        if (triggerPressed && !triggerReleased && tempTurret)
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


		RotatePlayer(rot);


        if (triggerPressed && !triggerReleased)
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
