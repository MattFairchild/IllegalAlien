using UnityEngine;
using System.Collections;

public class SpaceStationScript : Agent, IHittable
{

	// Use this for initialization
	void Start () {
        InitializeAgent();
	}
	
	// Update is called once per frame
	void Update () {

	}

    public void Hit(float damage, Agent attacker = null)
    {
        curHealth -= damage;
        if (curHealth <= 0)
        {
            Die();
            if (attacker)
            {
                attacker.IncreaseKillCount();
            }
        }
    }
    public override void IncreaseKillCount()
    {
        killCount++;
    }

    void Die()
    {
        Debug.Log("Game Over!!");
    }
}
