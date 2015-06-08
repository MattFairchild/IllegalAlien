﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GUI : MonoBehaviour {

	[SerializeField]protected Text score;
	[SerializeField]protected Image playerHealth;
	[SerializeField]protected Image playerShield;
	[SerializeField]protected Image playerShards;
	[SerializeField]protected Slider playerSpeed;
	[SerializeField]protected Image baseHealth;
	[SerializeField]protected Image baseShield;
	[SerializeField]protected Image bossHealth;
	[SerializeField]protected Image bossShield;

	public Player player;
	
	//ddd move to GUI script!

	// Use this for initialization
	void Start () {
		player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
	}
	
	// Update is called once per frame
	void Update () {
		score.text = "<size=50><i>Score</i></size>\n" + GameManager.score;
		playerShards.fillAmount = 1.0f*GameManager.curResources/GameManager.maxResources;
		playerSpeed.value = player.speed;
	}
}
