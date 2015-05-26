using UnityEngine;
using System.Collections;

//Class that should hold any type of infprmation needed in multiple other different scripts
public class GameManager : MonoBehaviour {
    public bool showRadius;
    public bool alwayShowRadius;
    public bool onlyNearest;

    //vectors that indicate the different directions on the screen
    public Vector3 screenUp;
    public Vector3 screenRight;
    public Vector3 screenNormal; //goes INTO the screen

    void Start()
    {
        showRadius = false;
        onlyNearest = false;
        alwayShowRadius = false;

        Transform cam = GameObject.FindGameObjectWithTag("MainCamera").transform;

        screenUp = cam.up;
        screenRight = cam.right;
        screenNormal = cam.forward;
    }
}
