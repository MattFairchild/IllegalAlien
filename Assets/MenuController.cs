using UnityEngine;
using System.Collections;

public class MenuController : MonoBehaviour {

    private GameObject mainMenu;
    private GameObject optionsMenu;

	// Use this for initialization
    void Awake()
    {
        mainMenu = GameObject.Find("Main Menu");
        optionsMenu = GameObject.Find("Options Menu");
    }


    public void changeFocus()
    {
        Vector3 mainPos = mainMenu.transform.position;
        Vector3 optionsPos = optionsMenu.transform.position;

        mainMenu.transform.position = Vector3.Lerp(mainMenu.transform.position, optionsPos, 1.0f);
        optionsMenu.transform.position = Vector3.Lerp(optionsMenu.transform.position, mainPos, 1.0f);
    }

}
