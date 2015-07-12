using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;

public abstract class EnemyScript : Agent, IHittable
{

    [SerializeField]
    protected int resources;
    [SerializeField]
    protected GameObject shardPrefab;
    protected Rigidbody rigid;
    [SerializeField]
    protected Image healthBar;

    protected GameObject spaceStation;
    protected GameObject player;
    protected GameObject target;

    protected NavMeshAgent agent;

    [SerializeField]
    protected GameObject bulletPrefab;
    [SerializeField]
    protected int numberOfShots = 1;
    [SerializeField]
    protected float damagePerShot = 0.5f;
    [SerializeField]
    protected float shootingFrequency = 1.0f;
    [SerializeField]
    protected float shootingRange = 15.0f;
    [SerializeField]
    protected float projectileSpeed = 25.0f;

    [SerializeField]
    protected Mesh[] meshes;

    protected new AudioSource[] audio;

    protected void InitializeEnemy()
    {
        InitializeAgent();
        player = GameManager.player.gameObject;//GameObject.FindGameObjectWithTag("Player");
        spaceStation = GameManager.spaceStation.gameObject;//GameObject.FindGameObjectWithTag("SpaceStation");
        healthBar.fillAmount = percentOfHealth;

        agent = GetComponent<NavMeshAgent>();
        rigid = gameObject.GetComponent<Rigidbody>();

        audio = GetComponents<AudioSource>();
        foreach (AudioSource source in audio)
        {
            source.maxDistance = shootingRange * 2f;
        }
        GameManager.incrementEnemyCount();

    }

    public void Hit(float damage, Agent attacker = null)
    {
        if (!alive)
        {
            return;
        }

        float tmp = healthBar.fillAmount;
        curHealth -= damage;
        percentOfHealth = curHealth / maxHealth;
        healthBar.fillAmount = percentOfHealth;
        //Color change?
        if (curHealth <= 0)
        {
            Die();
            if (attacker)
            {
                attacker.IncreaseKillCount();
            }
        }
        if (tmp < healthBar.fillAmount)
        {
            Debug.Log("?!?");
            Debug.Log(damage + " --> " + tmp + " to " + healthBar.fillAmount);
        }
        if (damage < 0)
            Debug.Log("!!!");
    }

    protected void Die()
    {
        alive = false;

        SpawnResources();

        GameManager.addScore((int)maxHealth);
        GameManager.decrementEnemyCount();

        GameObject explosion = Instantiate(deathExplosionPrefab, transform.position, Quaternion.identity) as GameObject;
		explosion.GetComponent<UnityStandardAssets.Effects.ParticleSystemMultiplier>().multiplier = GetTypeSpecificSizeModifier();
		Instantiate(deathSoundPrefab, transform.position, Quaternion.identity);
        Destroy(gameObject, 0.01f);
        //audio[0].PlayOneShot(deathClip, 5);

        /*
        GetComponent<Collider>().enabled = false;
        GetComponent<Renderer>().enabled = false;
        agent.enabled = false;
        this.enabled = false;
        foreach(Transform child in transform){
            child.gameObject.SetActive(false);
        }
        Destroy(gameObject, 2.5f);
        */
    }

    /*protected IEnumerator RotateOnDeath () {
        float startTime = Time.time;
        while (Time.time < startTime + 3) {
            transform.Rotate(Time.fixedDeltaTime * 90.0f / 3, 0, 0, Space.Self);
            yield return new WaitForFixedUpdate();
        }
    }*/

    protected void SpawnResources()
    {

        for (int i = 0; i < resources; i++)
        {
			Vector3 pos = 0.75f * GetTypeSpecificSizeModifier() * Random.insideUnitSphere; pos.y = 0;
			GameObject shard = (GameObject)Instantiate(shardPrefab, transform.position + pos, Random.rotationUniform);

            shard.GetComponent<ResourcesScript>().resources = 1;
            shard.GetComponent<MeshFilter>().mesh = meshes[i % meshes.Length];
            shard.GetComponent<MeshRenderer>().material = agent.GetComponent<MeshRenderer>().material;

            Rigidbody rbS = shard.GetComponent<Rigidbody>();

			Vector3 vel = agent.velocity + Random.insideUnitSphere; vel.y = 0;
			rbS.velocity = vel;
            rbS.angularVelocity = rigid.angularVelocity + Random.insideUnitSphere;
        }
    }

	protected float GetTypeSpecificSizeModifier () {
		return (this.GetType().Equals(typeof(InterceptorScript)) ? 1.5f : 2.5f);
	}

    public override void IncreaseKillCount()
    {
        killCount++;
    }

    void OnCollisionEnter(Collision collision)
    {
        InstantiateCollisionEffect(collision.contacts[0].point);

        switch (collision.gameObject.tag)
        {
            //case "Bullet":
            //now handled by bullet!
            //Hit (collision.gameObject.GetComponent<BulletScript>().damage);
            //break;
            case "Planet":
                Hit(0.25f * collision.relativeVelocity.magnitude);
                break;
            case "Player":
                Hit(0.2f * collision.relativeVelocity.magnitude);
                break;
            case "Turret":
                Hit(0.2f * collision.relativeVelocity.magnitude);
                break;
            case "Enemy":
                Hit(0.1f * collision.relativeVelocity.magnitude);
                break;
            default:
                break;
        }
        //Debug.DrawLine(collision.contacts[0].point, collision.contacts[0].point + 15f * collision.contacts[0].normal, Color.white);
    }
}
