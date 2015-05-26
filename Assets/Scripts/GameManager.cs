using UnityEngine;
using System.Collections;

//Class that should hold any type of infprmation needed in multiple other different scripts
public class GameManager : MonoBehaviour {

    public bool showRadius = false;
    public bool alwayShowRadius = false;
    public bool onlyNearest = false;

    //vectors that indicate the different directions on the screen
    public Vector3 screenUp;
    public Vector3 screenRight;
    public Vector3 screenNormal; //goes INTO the screen

    //Control Options
    public bool autoShoot = false;  //Shoot & rotate gun both with RT, only for gamepad

    void Start()
    {
        Transform cam = GameObject.FindGameObjectWithTag("MainCamera").transform;

        screenUp = cam.up;
        screenRight = cam.right;
        screenNormal = cam.forward;
    }
}
