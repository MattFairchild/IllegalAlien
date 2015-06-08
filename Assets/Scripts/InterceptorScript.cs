using UnityEngine;
using System.Collections;

public class InterceptorScript : EnemyScript
{

    private GameObject player;

	// Use this for initialization
     void Start() 
    {
        player = GameObject.FindGameObjectWithTag("Player");
        rigidbody = gameObject.GetComponent<Rigidbody>();
        currentHealth = maxHealth;
	}
	
	// Update is called once per frame
	void Update () 
    {
        Vector3 direction;
        direction = player.transform.position - transform.position;
        rigidbody.AddForce(direction.normalized);
	}
}
