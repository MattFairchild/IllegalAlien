using UnityEngine;
using System.Collections;

public class DestroyerScript : EnemyScript
{


	// Use this for initialization
	void Start ()
    {
		InitializeEnemy();
		target = spaceStation;
	}
	
	// Update is called once per frame
	void Update () 
    {
        agent.destination = target.transform.position;
        /*Vector3 direction;
        direction = target.transform.position - transform.position;
        rigidbody.AddForce(direction.normalized);*/
	}
}
