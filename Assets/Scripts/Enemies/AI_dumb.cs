﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class AI_dumb : MonoBehaviour {
	public bool big = false;

    public int resources;
	protected GameObject spaceStation;
	protected GameObject player;
	protected new Rigidbody rigidbody;
	protected float health = 1.0f;

    public GameObject scrapPrefab;

	[SerializeField]protected Image hpBar;

	// Use this for initialization
	void Start () {
		spaceStation = GameObject.FindGameObjectWithTag("SpaceStation");
		player = GameObject.FindGameObjectWithTag("Player");
		rigidbody = gameObject.GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 direction; 

		if(big){
			direction = spaceStation.transform.position - transform.position;
		}
		else{
			direction = player.transform.position - transform.position;
		}

		rigidbody.AddForce(direction.normalized);
	}

	void OnCollisionEnter (Collision collision) {
		if(collision.gameObject.tag == "Bullet"){
			health -= big ? 0.05f : 0.20f;
			if(health <= 0){
                GameObject scrap = (GameObject)Instantiate(scrapPrefab, transform.position, Quaternion.identity);
                ResourcesScript rs = scrap.GetComponent<ResourcesScript>();
                rs.resources = resources;
				Destroy(gameObject);
			}
			hpBar.fillAmount = health;
		}
	}
}
