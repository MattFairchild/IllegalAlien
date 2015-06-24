using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SpaceStationScript : Agent, IHittable {

	public Image healthBarOverlay;

	// Use this for initialization
	void Start () {
		InitializeAgent();
	}
	
	// Update is called once per frame
	void Update () {

	}

	void OnCollisionEnter (Collision collision) {
		InstantiateCollisionEffect (collision.contacts[0].point);
	}

	protected void Die () {
		Instantiate(deathExplosionPrefab, transform.position, Quaternion.identity);

		Debug.Log("Game Over!");
		GameManager.GameOver();
	}

    public void Hit (float damage, Agent attacker = null) {
        curHealth -= damage;
		percentOfHealth = curHealth/maxHealth;
        if (curHealth <= 0){
            Die();
            if (attacker){
                attacker.IncreaseKillCount();
            }
        }
    }

    public override void IncreaseKillCount () {
        killCount++;
    }
}
