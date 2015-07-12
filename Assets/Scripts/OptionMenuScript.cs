using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;

public class OptionMenuScript : MonoBehaviour {

    private GameObject difficultySlider;
    private GameObject soundSlider;
    private GameObject backButton;


    void Awake()
    {
        difficultySlider = GameObject.Find("Options Menu/Game Length Slider");
        difficultySlider = GameObject.Find("Options Menu/Sound Volume Slider");
        difficultySlider = GameObject.Find("Options Menu/Back");
    }


	// Use this for initialization
    void OnEnable()
    {
        EventSystem.current.SetSelectedGameObject(difficultySlider);
    }

}
