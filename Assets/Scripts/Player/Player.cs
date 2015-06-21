﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Player : Agent, IHittable {

	//[SerializeField]protected int maxResources;

	//[SerializeField]protected Image healthBar;
	//[SerializeField]protected Image shieldBar;
	//[SerializeField]protected Image shardsBar;
	//[SerializeField]protected Slider speedBar;
	public Image healthBarOverlay;
    public InputCapture input;

	protected Vector3 lastPos = Vector3.zero;
	public float speed;

	// Use this for initialization
	void Start () {
		InitializeAgent();
	}
	
	// Update is called once per frame
	/*void Update () {
		//healthBarOverlay.fillAmount = percentOfHealth;
		//healthBar.fillAmount = percentOfHealth;
		//shardsBar.fillAmount = (float)GameManager.curResources / (float)maxResources;
		//ddd move to GUI script!
	}*/

	void Update () {
		//speedBar.value = speed;
		//speed = 0.1f * (transform.position - lastPos).magnitude / Time.fixedDeltaTime;
		//lastPos = transform.position;
        if (input.placingTurret())
        {
            speed = 0.5f * input.getSpeed();
        }
        else
        {
            speed = input.getSpeed();
        }
	}

	protected void Die () {
		Debug.Log("Game over");
	}

	public void Hit (float damage, Agent attacker = null) {
		curHealth -= damage;
		percentOfHealth = curHealth/maxHealth;
		if(curHealth <= 0){
			Die();
			if (attacker){
				attacker.IncreaseKillCount();
			}
		}
	}

	public override void IncreaseKillCount () {
		killCount++;
	}

	void OnCollisionEnter (Collision collision) {
		switch(collision.gameObject.tag){
		case "Enemy":
			Hit (0.2f * collision.relativeVelocity.magnitude);
			break;
		case "Planet":
			Hit (0.5f * collision.relativeVelocity.magnitude);
			break;
		default:
			break;
		}
	}
}
