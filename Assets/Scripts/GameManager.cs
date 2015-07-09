using UnityEngine;
using System.Collections;


public class GameManager : MonoBehaviour {

    private static GameManager instance;

	public static float soundMasterVolume = 0.5f;
	public static float gameMasterDuration = 300;

	public static float lastTime = 0;
	public static int lastScore = 0;
	public static bool lastGameWon = false;


	public bool m_gameOver = false;
	public bool m_gamePaused = false;

	public Player m_player;
	public SpaceStationScript m_spaceStation;
	public GUIGame m_gui;

	public float gravitationalConstant;

    public bool m_showRadius = false;
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
	public int m_resourceCostPerTowerLevel = 20;
	public int m_score;
	public int m_enemyCountOnMap = 0;

	public bool controlsMixed = false;
	public float m_boostTimer;

	public float m_gameDuration = 300;
	public float m_startTime;
	public float m_endTime;
	
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
		Transform cam = Camera.main.transform;//GameObject.FindGameObjectWithTag("MainCamera").transform;
		SetSoundMasterVolume(soundMasterVolume);

        screenUp = cam.up;
        screenRight = cam.right;
        screenNormal = cam.forward;

        //set starting values
        m_curResources = 0;
		m_score = 0;
		m_startTime = Time.time;
		m_gameDuration = gameMasterDuration;
		m_endTime = m_startTime + m_gameDuration;
        m_boostTimer = 1.0f;
		StartCoroutine(EndGame());
	}

    public static float getGravitationalConstant()
    {
        return instance.gravitationalConstant;
    }

    public static bool showRadius {
		get { return instance.m_showRadius; }
		set { instance.m_showRadius = value; }
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

	public static void incrementEnemyCount () {
		enemyCount++;
	}

	public static void decrementEnemyCount () {
		enemyCount--;
	}

	public static int enemyCount {
		get { return instance.m_enemyCountOnMap; }
		protected set { instance.m_enemyCountOnMap = value; }
	}

	public static GUIGame gui {
		get { return instance.m_gui; }
		protected set { instance.m_gui = value; }
	}

	public static Player player {
		get { return instance.m_player; }
		protected set { instance.m_player = value; }
	}
	
	public static SpaceStationScript spaceStation {
		get { return instance.m_spaceStation; }
		protected set { instance.m_spaceStation = value; }
	}

	public static float boostTimer {
		get { return instance.m_boostTimer; }
		set{ instance.m_boostTimer = value; }
	}

	public static int maxCurAvailableTowerLevel {
		get { return curResources/resourceCostPerTowerLevel; }
	}

	public static int BuyMaxCurAvailableTower () {
		int lvl = maxCurAvailableTowerLevel;
		curResources -= lvl * resourceCostPerTowerLevel;
		return lvl;
	}

	public static int resourceCostPerTowerLevel {
		get { return instance.m_resourceCostPerTowerLevel; }
		protected set { instance.m_resourceCostPerTowerLevel = value; }
	}

	public static float startTime {
		get { return instance.m_startTime; }
	}
	
	public static float endTime {
		get { return instance.m_endTime; }
	}
	
	public static float gameDuration {
		get { return instance.m_gameDuration; }
	}

	public static bool gamePaused {
		get { return instance.m_gamePaused; }
	}

	/*public static int TowerResourceCost (int towerLevel) {
		switch(towerLevel){
		case 1:
			return 20;
		case 2:
			return 40;
		case 3:
			return 60;
		case 4:
			return 80;
		default:
			return 999; //-1 wouldn't be a good idea here, I guess
		}
	}*/

	public static bool mixedControls()
	{
		return instance.controlsMixed;
	}

	public static void GameOver (bool won = false) {
		if(instance.m_gameOver){
			return;
		}
		instance.m_gameOver = true;

        if (score > PlayerPrefs.GetInt("highscore", 0)){
            PlayerPrefs.SetInt("highscore", score);
        }

		lastTime = Time.time - startTime;
		lastScore = score;
		lastGameWon = won;

		instance.StartCoroutine(LoadLevelWithDelay(0, 2.5f));
	}

	public static void UnPauseGame () {
		instance.m_gamePaused = !instance.m_gamePaused;
		Time.timeScale = instance.m_gamePaused ? 0 : 1;
	}

	protected static IEnumerator LoadLevelWithDelay (int level, float delay) {
		yield return new WaitForSeconds(delay);
		Application.LoadLevel(level);
	}

	protected IEnumerator EndGame () {
		yield return new WaitForSeconds(m_gameDuration);
		GameOver(true);
	}

	public void ExitGame () {
		Debug.Log("Exiting game");
		#if UNITY_EDITOR
		UnityEditor.EditorApplication.isPlaying = false;
		#endif
		Application.Quit();
	}

	public static void QuitGame () {
		instance.ExitGame();
	}

	public void SetSoundMasterVolume (float newVolume) {

		soundMasterVolume = newVolume;//Mathf.Log10(Mathf.Clamp01(newVolume)*20);
		AudioListener.volume = soundMasterVolume;
	}

	public void SetGameMasterDuration (float newDuration) {
		gameMasterDuration = newDuration;
	}
}
