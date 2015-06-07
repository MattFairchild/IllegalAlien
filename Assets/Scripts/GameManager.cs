using UnityEngine;
using System.Collections;


public class GameManager : MonoBehaviour {

    private static GameManager instance;

    
    public float gravitationalConstant;

    public bool showRadius = false;
    public bool alwayShowRadius = false;
    public bool onlyNearest = false;

    //vectors that indicate the different directions on the screen
    public Vector3 screenUp;
    public Vector3 screenRight;
    public Vector3 screenNormal; //goes INTO the screen

    //Control Options
    public bool autoShoot = false;  //Shoot & rotate gun both with RT, only for gamepad

    public int resources;

    void Awake()
    {
        if (instance != null)
			return;
		
		instance = this;

    }
	// Use this for initialization
	void Start () {
	    Transform cam = GameObject.FindGameObjectWithTag("MainCamera").transform;

        screenUp = cam.up;
        screenRight = cam.right;
        screenNormal = cam.forward;

        //set starting values
        resources = 0;
	}
	

    public static float getGravitationalConstant()
    {
        return instance.gravitationalConstant;
    }

    public static bool getShowRadius()
    {
        return instance.showRadius;
    }

    public static void setShowRadius(bool show)
    {
        instance.showRadius = show;
    }

    public static bool getOnlyNearest()
    {
        return instance.onlyNearest;
    }

    public static bool getAlwaysShowRadius()
    {
        return instance.alwayShowRadius;
    }

    public static Vector3 getScreenUp()
    {
        return instance.screenUp;
    }
    public static Vector3 getScreenRight()
    {
        return instance.screenRight;
    }

    public static Vector3 getScreenNormal()
    {
        return instance.screenNormal;
    }

    public static bool getAutoShoot()
    {
        return instance.autoShoot;
    }

    public static int getResources()
    {
        return instance.resources;
    }

    public static void addResouces(int value)
    {
        instance.resources += value;

        if (instance.resources > 100)
            instance.resources = 100;

        else if (instance.resources < 0)
            instance.resources = 0;
    }
	
}
