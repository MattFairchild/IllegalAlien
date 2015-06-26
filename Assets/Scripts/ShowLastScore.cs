using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ShowLastScore : MonoBehaviour {

	[SerializeField]protected GameObject lastScoreUI;
	[SerializeField]protected Text  lastScoreText;

	// Use this for initialization
	void Start () {
		lastScoreUI.SetActive(GameManager.lastScore > 0);
		lastScoreText.text = (GameManager.lastGameWon ? "Congratulations, you won!" : "Oh dear, you were defeated...") 
														+ "\n Your score: " + GameManager.lastScore + ". Time: " + GameManager.lastTime.ToString("0")
                                                        + "\n Highscore: " + PlayerPrefs.GetInt("highscore", 0);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
