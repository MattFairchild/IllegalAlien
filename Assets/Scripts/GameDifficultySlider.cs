using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameDifficultySlider : MonoBehaviour {

    private Slider difficultySlider;

	// Use this for initialization
	void Awake () {
        difficultySlider = GetComponent<Slider>();
	}

    public void updateDifficulty()
    {
        int newVal = (int)difficultySlider.value;
        switch (newVal)
        {
            case 1:
                GameObject.Find("Options Menu/Game Difficulty Slider/Text").GetComponent<Text>().text = "EASY";
                break;
            case 2:
                GameObject.Find("Options Menu/Game Difficulty Slider/Text").GetComponent<Text>().text = "NORMAL";
                break;
            case 3:
                GameObject.Find("Options Menu/Game Difficulty Slider/Text").GetComponent<Text>().text = "HARD";
                break;
        }
    }
}
