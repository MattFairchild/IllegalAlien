using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public abstract class EnemyScript : Agent, IHittable {

	[SerializeField]protected int resources;
	[SerializeField]protected GameObject shardPrefab;
    protected Rigidbody rigid;
	[SerializeField]protected Image healthBar;

	protected GameObject spaceStation;
	protected GameObject player;
	protected GameObject target;

    protected NavMeshAgent agent;

    [SerializeField]protected GameObject bulletPrefab;
    [SerializeField]protected int numberOfShots = 1;
    [SerializeField]protected float damagePerShot = 0.5f;
    [SerializeField]protected float shootingFrequency = 1.0f;
    [SerializeField]protected float shootingRange = 15.0f;
    [SerializeField]protected float projectileSpeed = 25.0f;

    protected new AudioSource[] audio;

	protected void InitializeEnemy () {
		InitializeAgent();
		player = GameManager.player.gameObject;//GameObject.FindGameObjectWithTag("Player");
		spaceStation = GameManager.spaceStation.gameObject;//GameObject.FindGameObjectWithTag("SpaceStation");
		healthBar.fillAmount = percentOfHealth;

        agent = GetComponent<NavMeshAgent>();
		rigid = gameObject.GetComponent<Rigidbody>();

        audio = GetComponents<AudioSource>();
		foreach (AudioSource source in audio) {
			source.maxDistance = shootingRange * 2f;
		}
	}

	public void Hit (float damage, Agent attacker = null) {
		float tmp = healthBar.fillAmount;
		curHealth -= damage;
		percentOfHealth = curHealth/maxHealth;
		healthBar.fillAmount = percentOfHealth;
		//Color change?
		if (curHealth <= 0)	{
			Die ();
			if(attacker){
				attacker.IncreaseKillCount();
			}
		}
		if (tmp < healthBar.fillAmount){
			Debug.Log("?!?");
			Debug.Log(damage + " --> " + tmp + " to " + healthBar.fillAmount);
		}
		if (damage < 0)
			Debug.Log("!!!");
	}

	protected void Die () {
		int numberOfShards = Random.Range(2, 6); //returns values of 2-5!
		int remainingResourceCount = resources;

		for(int i = 0; i < numberOfShards && remainingResourceCount > 0; i++) {
			int resourceCountOfShard = Random.Range(Mathf.Min(i, remainingResourceCount), remainingResourceCount);
			GameObject shard = (GameObject)Instantiate(shardPrefab, transform.position, Random.rotationUniform);
			ResourcesScript rs = shard.GetComponent<ResourcesScript>();
			rs.resources = resourceCountOfShard;

			Rigidbody rbS = shard.GetComponent<Rigidbody>();

			rbS.velocity = agent.velocity + Random.insideUnitSphere;
			rbS.angularVelocity = rigid.angularVelocity + Random.insideUnitSphere;
		}

		GameManager.addScore((int)maxHealth);
        
		GameObject explosion = Instantiate(deathExplosionPrefab, transform.position, Quaternion.identity) as GameObject;
		explosion.GetComponent<UnityStandardAssets.Effects.ParticleSystemMultiplier>().multiplier = (this.GetType().Equals(typeof(InterceptorScript)) ? 1.5f : 2.5f);
		audio[0].PlayOneShot(deathClip, 5f);

		GetComponent<Collider>().enabled = false;
		GetComponent<Renderer>().enabled = false;
		agent.enabled = false;
		this.enabled = false;
		foreach(Transform child in transform){
			child.gameObject.SetActive(false);
		}
		Destroy(gameObject, 2.5f);
		//Destroy(gameObject, 0.01f);
	}

	/*protected IEnumerator RotateOnDeath () {
		float startTime = Time.time;
		while (Time.time < startTime + 3) {
			transform.Rotate(Time.fixedDeltaTime * 90.0f / 3, 0, 0, Space.Self);
			yield return new WaitForFixedUpdate();
		}
	}*/

	public override void IncreaseKillCount () {
		killCount++;
	}

    void OnCollisionEnter (Collision collision) {
		InstantiateCollisionEffect (collision.contacts[0].point);

        switch (collision.gameObject.tag){
		//case "Bullet":
			//now handled by bullet!
			//Hit (collision.gameObject.GetComponent<BulletScript>().damage);
			//break;
		case "Planet":
			Hit (0.25f * collision.relativeVelocity.magnitude);
			break;
		case "Player":
			Hit (0.2f * collision.relativeVelocity.magnitude);
			break;
		case "Turret":
			Hit (0.2f * collision.relativeVelocity.magnitude);
			break;
		case "Enemy":
			Hit (0.1f * collision.relativeVelocity.magnitude);
			break;
		default:
			break;
        }
		//Debug.DrawLine(collision.contacts[0].point, collision.contacts[0].point + 15f * collision.contacts[0].normal, Color.white);
    }
}
