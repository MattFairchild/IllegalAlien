using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ShowLastScore : MonoBehaviour {

	[SerializeField]protected GameObject lastScoreUI;
	[SerializeField]protected Text  lastScoreText;

	// Use this for initialization
	void Start () {
		lastScoreUI.SetActive(GameManager.lastScore > 0);
		string difficulty = "";
		switch(GameManager.difficultyMasterLevel){
		case 1:
			difficulty = "EASY";
			break;
		case 2:
			difficulty = "NORMAL";
			break;
		case 3:
			difficulty = "HARD";
			break;
		}

		lastScoreText.text = (GameManager.lastGameWon ? "Congratulations, you won!" : "Oh dear, you were defeated...") + "\n"
														+ "Your score: " + GameManager.lastScore + ". Time: " + GameManager.lastTime.ToString("0") + "\n"
                                                        + "Highscore: " + PlayerPrefs.GetInt(GameManager.GetHighscoreName(), 0) + " (" + difficulty + ")";
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
