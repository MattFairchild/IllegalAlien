using UnityEngine;
using System.Collections;

public class DestroyerScript : EnemyScript
{

    private GameObject spaceStation;

	// Use this for initialization
	void Start ()
    {
        spaceStation = GameObject.FindGameObjectWithTag("SpaceStation");
        rigidbody = gameObject.GetComponent<Rigidbody>();
        currentHealth = maxHealth;
	}
	
	// Update is called once per frame
	void Update () 
    {
        Vector3 direction;
        direction = spaceStation.transform.position - transform.position;
        rigidbody.AddForce(direction.normalized);
	}
}
