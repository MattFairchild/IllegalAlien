using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class ChangeFocusScript : MonoBehaviour
{

    private GameObject mainMenuButton;
    private GameObject optionsMenuButton;
    private GameObject helpMenuButton;

    // Use this for initialization
    void Start()
    {
        mainMenuButton = GameObject.Find("Main Menu/Start Game");
        optionsMenuButton = GameObject.Find("Options Menu/Game Difficulty Slider");
        helpMenuButton = GameObject.Find("Help Menu/Back");

        EventSystem.current.firstSelectedGameObject = mainMenuButton;
        EventSystem.current.SetSelectedGameObject(mainMenuButton);
    }

    public void switchFocus(int menuNumber)
    {

        switch (menuNumber)
        {
            case 0:
                EventSystem.current.firstSelectedGameObject = mainMenuButton;
                EventSystem.current.SetSelectedGameObject(mainMenuButton);
                break;

            case 1:
                EventSystem.current.firstSelectedGameObject = optionsMenuButton;
                EventSystem.current.SetSelectedGameObject(optionsMenuButton);
                break;
            case 2:
                EventSystem.current.firstSelectedGameObject = helpMenuButton;
                EventSystem.current.SetSelectedGameObject(helpMenuButton);
                break;

        }
    }
}
