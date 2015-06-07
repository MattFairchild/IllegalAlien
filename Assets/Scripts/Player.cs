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
	[SerializeField]protected Image healthBarOverlay;

	protected Vector3 lastPos = Vector3.zero;

	// Use this for initialization
	void Start () {
		curHealth = maxHealth;
	}
	
	// Update is called once per frame
	void Update () {
		float percentOfHealth = curHealth/maxHealth;
		healthBarOverlay.fillAmount = percentOfHealth;
		healthBar.fillAmount = percentOfHealth;
		shardsBar.fillAmount = (float)GameManager.getResources() / (float)maxResources;
	}

	void FixedUpdate () {
		speedBar.value = (transform.position - lastPos).magnitude * 6;
		lastPos = transform.position;
	}

	public void Hit (float damage) {
		curHealth -= damage;
		if(curHealth <= 0){
			Debug.Log("Game over!");
		}
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
