using UnityEngine;
using System.Collections;

public class InterceptorScript : EnemyScript
{
    

	// Use this for initialization
    void Start() 
    {
		InitializeEnemy();
		target = player;        
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
