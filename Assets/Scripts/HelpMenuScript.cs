using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class HelpMenuScript : MonoBehaviour
{


    private GameObject backButton;


    void Awake()
    {
        backButton = GameObject.Find("Options Menu/Back");
    }


    // Use this for initialization
    void OnEnable()
    {
        EventSystem.current.SetSelectedGameObject(backButton);
    }

}
