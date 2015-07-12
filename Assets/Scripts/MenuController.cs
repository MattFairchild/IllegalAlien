using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class MenuController : MonoBehaviour {

    private GameObject mainMenu;
    private GameObject optionsMenu;
    private bool change = false;
    Vector3 mainPos;
    Vector3 optionsPos;

	// Use this for initialization
    void Start()
    {
        mainMenu = GameObject.Find("Main Menu");
        optionsMenu = GameObject.Find("Options Menu");

        mainPos = mainMenu.transform.position;
        optionsPos = optionsMenu.transform.position;
    }


    void Update()
    {
        if (change)
        {
            mainMenu.transform.position = Vector3.Lerp(mainMenu.transform.position, optionsPos, 0.1f);
            optionsMenu.transform.position = Vector3.Lerp(optionsMenu.transform.position, mainPos, 0.1f);

            if (Vector3.Magnitude(mainMenu.transform.position - optionsPos) < 0.1f || Vector3.Magnitude(optionsMenu.transform.position - mainPos) < 0.1f)
            {
                mainPos = mainMenu.transform.position;
                optionsPos = optionsMenu.transform.position;
                change = false;
            }
        }
    }


    public void changeFocus()
    {
        change = true;
    }

}
