using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class MenuController : MonoBehaviour
{

    private GameObject mainMenu;
    private GameObject optionsMenu;
    private GameObject helpMenu;

    private bool change = false;
    private Vector3 mainPos;
    private Vector3 optionsPos;
    private Vector3 helpPos;

    private Vector3 safetyOffset;
    private Vector3 targetPos;
    private float lerpTime;

    // Use this for initialization
    void Start()
    {
        mainMenu = GameObject.Find("Main Menu");
        optionsMenu = GameObject.Find("Options Menu");
        helpMenu = GameObject.Find("Help Menu");

        mainPos = mainMenu.transform.position;
        optionsPos = optionsMenu.transform.position;
        helpPos = helpMenu.transform.position;

        GameObject camera = GameObject.Find("Main Camera");

        safetyOffset = mainMenu.transform.position - camera.transform.position + new Vector3(440, 0, -458);
    }


    void Update()
    {
        if (change)
        {
            lerpTime += Time.deltaTime;
            lerpTime = Mathf.Clamp01(lerpTime);

            mainMenu.transform.position = Vector3.Lerp(mainMenu.transform.position, mainPos - targetPos + safetyOffset, lerpTime);
            optionsMenu.transform.position = Vector3.Lerp(optionsMenu.transform.position, optionsPos - targetPos + safetyOffset, lerpTime);
            helpMenu.transform.position = Vector3.Lerp(helpMenu.transform.position, helpPos - targetPos + safetyOffset, lerpTime);

            if (lerpTime > 1f)
                change = false;



            //mainMenu.transform.position = Vector3.Lerp(mainMenu.transform.position, optionsPos, 0.1f);
            //optionsMenu.transform.position = Vector3.Lerp(optionsMenu.transform.position, mainPos, 0.1f);

            //if (Vector3.Magnitude(mainMenu.transform.position - optionsPos) < 0.1f || Vector3.Magnitude(optionsMenu.transform.position - mainPos) < 0.1f)
            //{
            //    mainPos = mainMenu.transform.position;
            //    optionsPos = optionsMenu.transform.position;
            //    change = false;
            //}
        }
    }


    public void changeFocus(int to)
    {
        change = true;
        lerpTime = 0;

        switch (to)
        {
            case 0:
                targetPos = mainPos;
                break;
            case 1:
                targetPos = optionsPos;
                break;
            case 2:
                targetPos = helpPos;
                break;
        }
    }

}
