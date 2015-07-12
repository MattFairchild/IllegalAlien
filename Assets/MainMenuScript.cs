using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class MainMenuScript : MonoBehaviour {

	// Use this for initialization
	void OnEnable () {
        GameObject firstButton = GameObject.Find("Main Menu/Start Game");
        EventSystem.current.SetSelectedGameObject(firstButton);
	}
	
}
