using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GUI : MonoBehaviour {

	[SerializeField]protected Text score;
	[SerializeField]protected Text timer;
	[SerializeField]protected Image timerCircle;
	
	[SerializeField]protected Image playerHealth;
	[SerializeField]protected Image playerShield;
	[SerializeField]protected Image playerShards;
	[SerializeField]protected Slider playerSpeed;

	[SerializeField]protected Image baseHealth;
	[SerializeField]protected Image baseShield;

	[SerializeField]protected Image bossHealth;
	[SerializeField]protected Image bossShield;

	protected Player player;
	protected Image playerHealthOverlay;

	//ddd move to GUI script!

	// Use this for initialization
	void Start () {
		player = GameManager.player;
		playerHealthOverlay = player.healthBarOverlay;
	}
	
	// Update is called once per frame
	void Update () {
		score.text = "<size=50><i>Score</i></size>\n" + GameManager.score;
		float timeLeft = GameManager.endTime - Time.time;
		timer.text = "<size=50><i>Time</i></size>\n" + (int)timeLeft;
		timerCircle.fillAmount = timeLeft / GameManager.gameDuration;
		playerShards.fillAmount = 1.0f*GameManager.curResources/GameManager.maxResources;
		playerSpeed.value = player.speed;
		playerHealth.fillAmount = playerHealthOverlay.fillAmount = player.percentOfHealth;
	}
}
