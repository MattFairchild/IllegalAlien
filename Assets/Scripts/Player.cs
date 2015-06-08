using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Player : MonoBehaviour {

	[SerializeField]protected float maxHealth;
	[SerializeField]protected float curHealth;

	[SerializeField]protected int maxResources;

	[SerializeField]protected Image healthBar;
	[SerializeField]protected Image shieldBar;
	[SerializeField]protected Image shardsBar;
	[SerializeField]protected Slider speedBar;

	protected Vector3 lastPos = Vector3.zero;

	// Use this for initialization
	void Start () {
		curHealth = maxHealth;
	}
	
	// Update is called once per frame
	void Update () {
		healthBar.fillAmount = curHealth/maxHealth;
		shardsBar.fillAmount = (float)GameManager.getResources() / (float)maxResources;
	}

	void FixedUpdate () {
		speedBar.value = (transform.position - lastPos).magnitude * 6;
		lastPos = transform.position;
	}

	protected void Hit (float damage) {
		curHealth -= damage;
		if(curHealth <= 0){
			Debug.Log("Game over!");
		}
	}

	void OnCollisionEnter (Collision collision) {
		switch(collision.gameObject.tag){
		case "Enemy":
			Hit (1);
			break;
		case "Planet":
			Hit (2);
			break;
		default:
			break;
		}
	}
}
