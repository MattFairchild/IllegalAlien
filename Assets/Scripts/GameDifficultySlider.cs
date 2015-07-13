using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameDifficultySlider : MonoBehaviour {

    private Slider difficultySlider;
	[SerializeField]protected Text sliderText;

	// Use this for initialization
	void Awake () {
        difficultySlider = GetComponent<Slider>();
		//sliderText = GameObject.Find("Options Menu/Game Difficulty Slider/Text").GetComponent<Text>();
	}

    public void updateDifficulty()
    {
        int newVal = (int)difficultySlider.value;
        switch (newVal)
        {
            case 1:
				sliderText.text = "EASY";
                break;
            case 2:
				sliderText.text = "NORMAL";
                break;
            case 3:
                sliderText.text = "HARD";
                break;
        }
		GameManager.difficultyMasterLevel = newVal;
    }
}
