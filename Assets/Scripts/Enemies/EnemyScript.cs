﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public abstract class EnemyScript : Agent, IHittable {

	[SerializeField]protected int resources;
	[SerializeField]protected GameObject scrapPrefab;
    protected new Rigidbody rigidbody;
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
		rigidbody = gameObject.GetComponent<Rigidbody>();

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
		GameObject scrap = (GameObject)Instantiate(scrapPrefab, transform.position, Quaternion.identity);
		ResourcesScript rs = scrap.GetComponent<ResourcesScript>();
		rs.resources = resources;

		Rigidbody rbE = gameObject.GetComponent<Rigidbody>();
		Rigidbody rbS = rs.GetComponent<Rigidbody>();
		rbS.velocity = rbE.velocity;
		rbS.angularVelocity = rbE.angularVelocity;

		GameManager.addScore((int)maxHealth);
        
		Destroy(gameObject, 0.01f);
		enabled = false;
	}

	public override void IncreaseKillCount () {
		killCount++;
	}

    void OnCollisionEnter (Collision collision) {
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
