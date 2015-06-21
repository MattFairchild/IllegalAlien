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
        agent.destination = spaceStation.transform.position;
	}
}
