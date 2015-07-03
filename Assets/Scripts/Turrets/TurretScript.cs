﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class TurretScript : Agent, IHittable {

	/*Gravity stuff*/
    protected Vector3 direction = Vector3.zero;
	protected float distance = 0;
	protected float orbitalSpeed;
	[SerializeField]protected bool affectedByGravity = false;
	[SerializeField]protected bool launched = false;
    public LineRenderer lR;
    public GameObject[] planets;


	/*References stuff*/
	[SerializeField]protected Rigidbody rb;
	[SerializeField]protected GameObject turretObject;
	[SerializeField]protected TrailRenderer trail;
    [SerializeField]protected new CapsuleCollider collider;
	[SerializeField]protected SphereCollider trigger;
	[SerializeField]protected GameObject bulletPrefab;

	/*Shooting stuff*/
	[SerializeField]protected int numberOfShots = 1;
	[SerializeField]protected float damagePerShot = 0.5f;
	[SerializeField]protected float shootingFrequency = 1.0f;
	[SerializeField]protected float shootingRange = 10.0f;
	[SerializeField]protected float directionalInaccuracyExtent = 7.5f;
	[SerializeField]protected float projectileSpeed = 25.0f;
    [SerializeField]protected float collidingPanicRange = 3.5f;
	[SerializeField]protected float preferredRange = 2.5f;
	
	protected float ComputeEstimatedDPS () {
		return numberOfShots * damagePerShot * shootingFrequency * (100 - 2*directionalInaccuracyExtent)/100;
	}

	protected List<EnemyScript> enemiesInRange = new List<EnemyScript>();

	/*more stuff*/
	[SerializeField]protected int towerLevel = 1;
	protected new AudioSource audio;

	/*GUI stuff*/
	[SerializeField]protected Canvas towerGUICanvas;
	[SerializeField]protected Text textHealth;
	[SerializeField]protected Text textKillCount;
	[SerializeField]protected Text textRange;
	[SerializeField]protected Text textDPS;
	[SerializeField]protected Text textState;
	[SerializeField]protected Text textLvl;
	[SerializeField]protected Image imageRange;
    protected bool guiVisible = false;
    protected bool playerInRange = false;

    // Use this for initialization
    void Start(){
		InitializeAgent();
		InitializeTower();
		InitializeTowerGUI();

        Color color1 = new Color(0.0f, 230.0f, 200.0f, 100.0f);
        Color color2 = new Color(0.0f, 230.0f, 100.0f, 100.0f);

        lR = gameObject.AddComponent<LineRenderer>();
        lR.material = new Material(Shader.Find("Particles/Additive"));
        lR.SetColors(color1, color2);

        lR.SetVertexCount(200);
        lR.SetWidth(0.1f, 0.08f);
    }

	protected void InitializeTower () {
		planets = GameObject.FindGameObjectsWithTag("Planet");
		
		rb.velocity = Vector3.zero;
		trigger.radius = shootingRange;
		
		audio = GetComponent<AudioSource>();
		audio.maxDistance = shootingRange * 2;
	}

	protected void InitializeTowerGUI () {
		textHealth.text = "Health := " + maxHealth.ToString("0.00");
		textKillCount.text = "KillCount := 0";
		textRange.text = "Range := " + shootingRange.ToString("0.00");
		textDPS.text = "est. DPS :~ " + ComputeEstimatedDPS().ToString("0.00");
		textState.text = "State := <color=#22B2FF>" + "unlaunched" + "</color>";
		textLvl.text = "<color=orange>" + towerLevel + "</color>\n<size=20><color=white>LVL</color></size>";
		imageRange.rectTransform.localScale *= shootingRange/5;
        StartCoroutine(FadeTowerGUI());
	}

    void FixedUpdate()
    {
        if (playerInRange)
        { 
            if (!GameManager.player.input.placingTurret())
            {
                guiVisible = false;
            }
            else if (GameManager.player.input.placingTurret())
            {
                guiVisible = true;
            }        
        }


        if (launched && affectedByGravity)
        {
            Gravity();
            lR.enabled = false;
        }
        else
        {
            showProjection();
        }

    }

    public void SetVelocity(Vector3 vel)
    {
        affectedByGravity = true;
        rb.velocity = vel;
		launched = true;
		
		StartCoroutine(Fight());
		StartCoroutine(FightSpreadshot());
		textState.text = "State := <color=#22B2FF>" + "active" + "</color>";
    }

    public void setAffectedByGravity(bool affected)
    {
        affectedByGravity = affected;
    }

    private void Gravity()
    {

        foreach (GameObject planet in planets)
        {
            //calculate the gravity of every planet into the equation 
            Rigidbody planetRb = planet.GetComponent<Rigidbody>();
            direction = Vector3.Normalize(planet.transform.position - this.transform.position);
            distance = Vector3.Magnitude(planet.transform.position - this.transform.position);

            //limit range of gravity
            if (Vector3.Distance(transform.position, planet.transform.position) <= planet.GetComponent<PlanetScript>().range)
            {
                rb.velocity += ((GameManager.getGravitationalConstant() * planetRb.mass / Mathf.Pow(distance, 2)) * direction) * Time.deltaTime;
            }           
        }


    }

	public override void IncreaseKillCount () {
		killCount++;
		textKillCount.text = "KillCount := <color=#22B2FF>" + killCount + "</color>";
	}

    // Collision with other objects
    void OnCollisionEnter (Collision collision) {
		InstantiateCollisionEffect (collision.contacts[0].point);

		switch(collision.gameObject.tag){
		case "Planet":
			Die();
			break;
		case "Turret":
		case "Player":
		case "Enemy":
			Hit (0.2f * collision.relativeVelocity.magnitude);
            collision.gameObject.GetComponent<IHittable>().Hit(0.2f * collision.relativeVelocity.magnitude);
			break;
		default:
			break;
			//TODO: Collision with other objects!!
        }
    }

	void OnTriggerEnter (Collider other) {
		switch(other.tag){
		case "Enemy":
			EnemyScript newEnemy = other.GetComponent<EnemyScript>();
			if(newEnemy && !enemiesInRange.Contains(newEnemy)){
				enemiesInRange.Add(newEnemy);
			}
			break;
		case "Player":
            playerInRange = true;
			break;
		}
	}

	void OnTriggerExit (Collider other) {
		switch(other.tag){
		case "Enemy":
			enemiesInRange.Remove(other.GetComponent<EnemyScript>());
			break;
		case "Player":
            playerInRange = false;
			break;
		}
	}

    protected IEnumerator FadeTowerGUI () {
        //towerGUICanvas.enabled = visible;
        CanvasGroup gui = towerGUICanvas.GetComponent<CanvasGroup>();
        while (enabled) {
            gui.alpha = Mathf.Clamp01(gui.alpha + (guiVisible ? Time.fixedDeltaTime : -Time.fixedDeltaTime));
            yield return new WaitForFixedUpdate();
        }
    }

	protected IEnumerator Fight () {
		while(enabled){
			while(enemiesInRange.Count == 0){
				yield return new WaitForSeconds(0.1f);
			}
			enemiesInRange.RemoveAll(item => item == null);
			if(enemiesInRange.Count > 0){
				EnemyScript enemy = PickEnemy();
				if(enemy){
					audio.PlayOneShot(audio.clip);
					for(int i = 0; (i < numberOfShots) && enemy; i++){
						FireProjectile(enemy);
						//Debug.Log(i+1);
                        yield return new WaitForSeconds(0.1f);
					}
				}
			}
			yield return new WaitForSeconds(1/shootingFrequency);
		}
	}

	protected IEnumerator FightSpreadshot () {
		while(enabled){
			if(enemiesInRange.Count >= 3){
				FireSpreadshot();
				audio.PlayOneShot(audio.clip);
			}
			yield return new WaitForSeconds(16 - 2*towerLevel);
		}
	}

	protected EnemyScript PickEnemy () {
		Transform spaceStation = GameManager.spaceStation.transform;
		EnemyScript nearestToBaseEnemy = null;
        EnemyScript nearestToTowerEnemy = null;
        EnemyScript nearestCollidingEnemy = null;
		float minDistToBase = float.PositiveInfinity;
        float minDistToTower = float.PositiveInfinity;
        float minDistToColliding = float.PositiveInfinity;

		foreach(EnemyScript enemy in enemiesInRange){
			float curDistToBase = Vector3.Distance(spaceStation.position, enemy.transform.position);
            float curDistToTower = Vector3.Distance(transform.position, enemy.transform.position);
            
			if(curDistToBase < minDistToBase){
				minDistToBase = curDistToBase;
				nearestToBaseEnemy = enemy;
			}

            if(curDistToTower < minDistToTower){
                minDistToTower = curDistToTower;
                nearestToTowerEnemy = enemy;
            }

            Ray ray = new Ray(enemy.transform.position, transform.forward);
            RaycastHit hit = new RaycastHit();
            if(curDistToTower < minDistToColliding && collider.Raycast(ray, out hit, shootingRange)){
                nearestCollidingEnemy = enemy;
                minDistToColliding = curDistToTower;
            }
		}

        if(minDistToColliding < collidingPanicRange || percentOfHealth < lowHealthPercentage) {
            return nearestCollidingEnemy;
        }
        else if(minDistToTower < preferredRange  || percentOfHealth < lowHealthPercentage) {
            return nearestToTowerEnemy;
        }
        else {
		    return nearestToBaseEnemy;
        }
	}

	protected void FireProjectile (EnemyScript target) {
		Vector3 dir = ComputeFiringDirection(target);
		BulletScript bs = (GameObject.Instantiate(bulletPrefab, ComputeProtectilePosition(dir), Quaternion.LookRotation(dir)) as GameObject).GetComponent<BulletScript>();
		bs.sender = this;
		bs.damage = damagePerShot;
		if(towerLevel == 4){
			bs.targetSeeking = true;
			bs.target = target.transform;
		}
		//bs.faction = GameManager.Factions.Enemy;
	}

	protected Vector3 ComputeFiringDirection (EnemyScript target) {
		float dist = Vector3.Distance(target.transform.position, transform.position);
		//Vector3 estimatedMovement = target.gameObject.GetComponent<Rigidbody>().velocity * dist / projectileSpeed;
        Vector3 estimatedMovement = target.gameObject.transform.forward * target.gameObject.GetComponent<Rigidbody>().velocity.magnitude * dist / projectileSpeed;
		Vector3 dir = ((target.transform.position + 1.2f * estimatedMovement) - transform.position).normalized;
		//apply directional inaccuracy
		float curInaccuracyAngle = Random.Range(-directionalInaccuracyExtent, +directionalInaccuracyExtent);
		dir = Quaternion.Euler(0, curInaccuracyAngle, 0) * dir;
		return dir;
	}

	protected Vector3 ComputeProtectilePosition (Vector3 firingDirection) {
		Vector3 tmp = transform.localScale;
		float safetyDistance = Mathf.Max(Mathf.Max(tmp.x, tmp.y), tmp.z) + bulletPrefab.transform.localScale.y / 2;
		return (this.transform.position + firingDirection * safetyDistance);
	}

	protected void FireSpreadshot () {
		float angle = Random.value * 360;
		int shots = 4 + towerLevel;
		for(int i = 0; i < shots; i++){
			Vector3 dir = Quaternion.AngleAxis(angle + i*360.0f/shots, Vector3.up) * Vector3.forward;
			BulletScript bs = (GameObject.Instantiate(bulletPrefab, ComputeProtectilePosition(dir), Quaternion.LookRotation(dir)) as GameObject).GetComponent<BulletScript>();
			bs.damage = 2 * damagePerShot;
		}
	}

	public void Hit (float damage, Agent attacker = null) {
		if(!alive){
			return;
		}

		curHealth -= damage;
        percentOfHealth = curHealth / maxHealth;
		textHealth.text = "Health := <color=#22B2FF>" + curHealth.ToString("0.00") + "</color>";
		//healthBar.fillAmount = curHealth/maxHealth;
		//Color change?
		if (curHealth <= 0)	{
			Die ();
			if(attacker){
				attacker.IncreaseKillCount();
			}
		}
	}

	protected void Die () {
		alive = false;

		Instantiate(deathExplosionPrefab, transform.position, Quaternion.identity);

		Destroy(turretObject);
		rb.isKinematic = true;
		trail.autodestruct = true;
		trail.time *= 0.5f;
		trail.startWidth *= 1.5f;
		trail.material.SetColor("_TintColor", new Color(1.0f, 0.0f, 0.125f, 1));
		trigger.enabled = false;
		imageRange.enabled = false;
		textState.text = "State := <color=#22B2FF>" + "dead" + "</color>";
		Destroy(this);
	}


    protected void showProjection()
    {
        Vector3 position = transform.position;
        Vector3 vel = GameManager.player.getCurrentVelocity();
        float speedPercent;

        if (GameManager.player.speed > 0.0f)
        {
            speedPercent = GameManager.player.speed / GameManager.player.maxSpeed;
        }
        else
        {
            speedPercent = 1.0f;
        }
        
        int numPoints = 200 + ((int)(600 * (1.0f - speedPercent)));

        lR.SetVertexCount(numPoints);

        for (int i = 0; i < numPoints; i++)
        {
            position = nextStep(position, ref vel);
            lR.SetPosition(i, position);
        }
    }

    protected Vector3 nextStep(Vector3 pos, ref Vector3 velocity)
    {

        foreach (GameObject planet in planets)
        {
            //calculate the gravity of every planet into the equation 
            Rigidbody planetRb = planet.GetComponent<Rigidbody>();
            direction = Vector3.Normalize(planet.transform.position - pos);
            distance = Vector3.Magnitude(planet.transform.position - pos);

            //limit range of gravity
            if (Vector3.Distance(pos, planet.transform.position) <= planet.GetComponent<PlanetScript>().range)
            {
                velocity += ((GameManager.getGravitationalConstant() * planetRb.mass / Mathf.Pow(distance, 2)) * direction) * Time.deltaTime;
            }
        }

        return pos + Time.fixedDeltaTime*velocity;
    }
}
