using UnityEngine;
using System.Collections;


public class GameManager : MonoBehaviour {

    private static GameManager instance;

	public Player m_player;
	public GameObject m_spaceStation;
	public GUI m_gui;

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

    public int m_curResources;
	public int m_maxResources = 100;
	public int m_score;

	public bool controlsMixed = false;
	public float boostTimer;

	public enum Factions{
		Player,
		Enemy,
		None
	}
	
	public static string[] FactionTagsPlayer = {"Player", "Turret", "SpaceStation"};
	public static string[] FactionTagsEnemy = {"Enemy"};
	public static string[] FactionTagsNeutral = {"Planet"};

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
        m_curResources = 0;
		m_score = 0;
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

    public static int curResources {
		get { return instance.m_curResources; }
		protected set { instance.m_curResources = value; }
    }

	public static int maxResources {
		get { return instance.m_maxResources; }
		protected set { instance.m_maxResources = value; }
	}

    public static void addResouces (int value) {
        if(value > 0){
			curResources += value;
			Mathf.Clamp(curResources, 0, maxResources);
		}
    }

	public static int score {
		get { return instance.m_score; }
		protected set { instance.m_score = value; }
	}

	public static void addScore (int value) {
		if(value > 0){
			score += value;
		}
	}

	public static Player player {
		get { return instance.m_player; }
		protected set { instance.m_player = value; }
	}

	public static GUI gui {
		get { return instance.m_gui; }
		protected set { instance.m_gui = value; }
	}

	public static GameObject spaceStation {
		get { return instance.m_spaceStation; }
		protected set { instance.m_spaceStation = value; }
	}

	public static void setBoost(float val)
	{
		instance.boostTimer = val;
	}

	public static bool mixedControls()
	{
		return instance.controlsMixed;
	}

	public static float getBoostTime()
	{
		return instance.boostTimer;
	}
}
