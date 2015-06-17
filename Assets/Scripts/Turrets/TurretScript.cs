using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class TurretScript : Agent, IHittable {

	/*Gravity stuff*/
    public Vector3 direction;
    public float distance;
    public float orbitalSpeed;
    public bool affectedByGravity = false;
	[SerializeField]protected bool launched = false;
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

	protected List<EnemyScript> enemiesInRange = new List<EnemyScript>();

	/*more stuff*/
	[SerializeField]protected int towerLevel = 1;
	//[SerializeField]protected int shardCost = 5;
	protected int killCount = 0;
	protected new AudioSource audio;

	/*GUI stuff*/
	[SerializeField]protected Canvas towerGUI;
	[SerializeField]protected Text textHealth;
	[SerializeField]protected Text textKillCount;
	[SerializeField]protected Text textRange;
	[SerializeField]protected Text textDPS;
	[SerializeField]protected Text textState;
	[SerializeField]protected Text textLvl;

    // Use this for initialization
    void Start(){
		InitializeAgent();
        planets = GameObject.FindGameObjectsWithTag("Planet");
        direction = Vector3.zero;
        rb.velocity = Vector3.zero;

		trigger.radius = shootingRange;

		audio = GetComponent<AudioSource>();
        audio.maxDistance = shootingRange * 2;
    }

    void FixedUpdate()
    {
        if (launched && affectedByGravity)
        {
            Gravity();
        }
    }

    public void SetVelocity(Vector3 vel)
    {
        affectedByGravity = true;
        rb.velocity = vel;
		launched = true;
		
		StartCoroutine(Fight());
		StartCoroutine(FightSpreadshot());
    }

    public void setAffectedByGravity(bool affected)
    {
        affectedByGravity = affected;
    }

    private void Gravity()
    {
        //temp object for the case that we only consider the nearest planet.
        //start it off as the first object if there are any panets at all
        GameObject nearestPlanet = this.gameObject;
        float tempDistance = 0.0f;
        if (planets.Length > 0)
        {
            nearestPlanet = planets[0];
            tempDistance = Vector3.Magnitude(nearestPlanet.transform.position - this.transform.position);
        }

        foreach (GameObject planet in planets)
        {
            //if we only want the nearest panet to attract, then use the loop only to find the nearest planet
            if (GameManager.getOnlyNearest() && planets.Length > 0)
            {
                if (Vector3.Magnitude(planet.transform.position - this.transform.position) < tempDistance)
                {
                    nearestPlanet = planet;
                    tempDistance = Vector3.Magnitude(planet.transform.position - this.transform.position);
                }
            }
            //otherwise calculate the gravity of every planet into the equation
            else
            { 
                Rigidbody planetRb = planet.GetComponent<Rigidbody>();
                direction = Vector3.Normalize(planet.transform.position - this.transform.position);
                distance = Vector3.Magnitude(planet.transform.position - this.transform.position);

                //limit range of gravity depending on orbital speed
                orbitalSpeed = Mathf.Sqrt(GameManager.getGravitationalConstant() * planetRb.mass / distance);
                if (orbitalSpeed >= 1.5f)
                {
                    rb.velocity += ((GameManager.getGravitationalConstant() * planetRb.mass / Mathf.Pow(distance, 2)) * direction) * Time.deltaTime;
                }
                       
            }
        }


        //if only the nearest planet should be used, calc same equation as above, but for the nearest planet only
        if (GameManager.getOnlyNearest() && planets.Length > 0)
        {
            Rigidbody planetRb = nearestPlanet.GetComponent<Rigidbody>();
            direction = Vector3.Normalize(nearestPlanet.transform.position - this.transform.position);
            distance = Vector3.Magnitude(nearestPlanet.transform.position - this.transform.position);
            rb.velocity += ((GameManager.getGravitationalConstant() * planetRb.mass / Mathf.Pow(distance, 2)) * direction) * Time.deltaTime;         
        }

    }

    // Collision with other objects
    void OnCollisionEnter (Collision collision) {
		switch(collision.gameObject.tag){
		case "Planet":
			Die();
			break;
		case "Turret":
		case "Player":
		case "Enemy":
			Hit (0.2f * collision.relativeVelocity.magnitude);
			break;
		default:
			break;
			//TODO: Collision with other objects!!
        }
    }

	void OnTriggerEnter (Collider other) {
		if(other.tag == "Enemy"){
			EnemyScript newEnemy = other.GetComponent<EnemyScript>();
			if(newEnemy && !enemiesInRange.Contains(newEnemy)){
				enemiesInRange.Add(newEnemy);
			}
		}
	}

	void OnTriggerExit (Collider other) {
		if(other.tag == "Enemy"){
			enemiesInRange.Remove(other.GetComponent<EnemyScript>());
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
						Debug.Log(i+1);
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

	public void Hit (float damage) {
		curHealth -= damage;
        percentOfHealth = curHealth / maxHealth;
		//healthBar.fillAmount = curHealth/maxHealth;
		//Color change?
		if (curHealth <= 0)	{
			Die ();
		}
	}

	protected void Die () {
		//Destroy(this.gameObject);
		Destroy(turretObject);
		rb.isKinematic = true;
		trail.autodestruct = true;
		trail.time *= 0.5f;
		trail.startWidth *= 1.5f;
		trail.material.SetColor("_TintColor", new Color(1.0f, 0.0f, 0.125f, 1));
		trigger.enabled = false;
		Destroy(this);
	}

}
