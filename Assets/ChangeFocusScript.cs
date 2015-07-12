using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class ChangeFocusScript : MonoBehaviour {

    private GameObject mainMenuButton;
    private GameObject optionsMenuButton;
    private bool mainMenu = true;

	// Use this for initialization
	void Start () {
        mainMenuButton = GameObject.Find("Main Menu/Start Game");
        optionsMenuButton = GameObject.Find("Options Menu/Game Difficulty Slider");

        EventSystem.current.firstSelectedGameObject = mainMenuButton;
        EventSystem.current.SetSelectedGameObject(mainMenuButton);
	}

    public void switchFocus()
    {
        mainMenu = !mainMenu;

        if (mainMenu)
        {
            EventSystem.current.firstSelectedGameObject = mainMenuButton;
            EventSystem.current.SetSelectedGameObject(mainMenuButton);
        }
        else
        {
            EventSystem.current.firstSelectedGameObject = optionsMenuButton;
            EventSystem.current.SetSelectedGameObject(optionsMenuButton);
        }

    }
}
