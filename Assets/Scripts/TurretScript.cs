using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TurretScript : Agent, IHittable {

    public Rigidbody rb;
    public Vector3 direction;
    public float distance;
    public float orbitalSpeed;
    public bool affectedByGravity = false;
	protected bool launched = false;
    public GameObject[] planets;

	[SerializeField]protected GameObject turretObject;
	[SerializeField]protected TrailRenderer trail;
	[SerializeField]protected SphereCollider trigger;

	[SerializeField]protected GameObject bulletPrefab;

	protected List<EnemyScript> enemiesInRange = new List<EnemyScript>();
	[SerializeField]protected float damagePerShot = 0.5f;
	[SerializeField]protected float shootingInterval = 1.0f;
	[SerializeField]protected float shootingRange = 5.0f;

	[SerializeField]protected float projectileSpeed = 25.0f;

	protected new AudioSource audio;

    // Use this for initialization
    void Start(){
		InitializeAgent();
        planets = GameObject.FindGameObjectsWithTag("Planet");
        direction = Vector3.zero;
        rb.velocity = Vector3.zero;

		trigger.radius = shootingRange;

		audio = GetComponent<AudioSource>();
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
					FireProjectile(enemy);
				}
			}
			yield return new WaitForSeconds(shootingInterval);
		}
	}

	protected EnemyScript PickEnemy () {
		Transform spaceStation = GameManager.spaceStation.transform;
		EnemyScript nearestEnemy = null;
		float minDist = float.PositiveInfinity;
		foreach(EnemyScript enemy in enemiesInRange){
			float curDist = Vector3.Distance(spaceStation.position, enemy.transform.position);
			if(curDist < minDist){
				minDist = curDist;
				nearestEnemy = enemy;
			}
		}
		return nearestEnemy;
	}

	protected void FireProjectile (EnemyScript target) {
		audio.PlayOneShot(audio.clip);
		Vector3 dir = ComputeFiringDirection(target);
		BulletScript bs = (GameObject.Instantiate(bulletPrefab, ComputeProtectilePosition(dir), Quaternion.LookRotation(dir)) as GameObject).GetComponent<BulletScript>();
		bs.damage = damagePerShot;
		//bs.targetSeeking = true;
		bs.target = target.transform;
		//bs.faction = GameManager.Factions.Enemy;
	}

	protected Vector3 ComputeFiringDirection (EnemyScript target) {
		float dist = Vector3.Distance(target.transform.position, transform.position);
		Vector3 estimatedMovement = target.gameObject.GetComponent<Rigidbody>().velocity * dist / projectileSpeed;
		Vector3 dir = ((target.transform.position + estimatedMovement) - transform.position).normalized;
		//apply directional inaccuracy
		float inaccuracyAngle = Random.Range(-5.0f, +5.0f);
		dir = Quaternion.Euler(0, inaccuracyAngle, 0) * dir;
		return dir;
	}

	protected Vector3 ComputeProtectilePosition (Vector3 firingDirection) {
		Vector3 tmp = transform.localScale;
		float safetyDistance = Mathf.Max(Mathf.Max(tmp.x, tmp.y), tmp.z) + bulletPrefab.transform.localScale.y / 2;
		return (this.transform.position + firingDirection * safetyDistance);
	}

	public void Hit (float damage) {
		curHealth -= damage;
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
