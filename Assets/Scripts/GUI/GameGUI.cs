using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameGUI : MonoBehaviour
{

    [SerializeField]
    protected RectTransform scoreGUIParent;
    [SerializeField]
    protected Text score;
    [SerializeField]
    protected RectTransform enemiesGUIParent;
    [SerializeField]
    protected Text enemies;
    [SerializeField]
    protected RectTransform timerGUIParent;
    [SerializeField]
    protected Text timer;
    [SerializeField]
    protected Image timerCircle;

    [SerializeField]
    protected RectTransform playerGUIParent;
    [SerializeField]
    protected Image playerHealth;
    [SerializeField]
    protected Image playerShield;
    [SerializeField]
    protected Image playerShards;
    [SerializeField]
    protected Slider playerSpeed;

    [SerializeField]
    protected RectTransform baseGUIParent;
    [SerializeField]
    protected Image baseHealth;
    [SerializeField]
    protected Image baseShield;

    [SerializeField]
    protected RectTransform bossGUIParent;
    [SerializeField]
    protected Image bossHealth;
    [SerializeField]
    protected Image bossShield;

    [SerializeField]
    protected Image pauseOverlay;
    [SerializeField]
    protected RectTransform pauseImage;

    protected Player player;
    protected SpaceStationScript spaceStation;
    protected Image playerHealthOverlay;
    protected Image baseHealthOverlay;

    protected bool lastPaused = false;

    // Use this for initialization
    void Start()
    {
        player = GameManager.player;
        spaceStation = GameManager.spaceStation;
        playerHealthOverlay = player.healthBarOverlay;
        baseHealthOverlay = spaceStation.healthBarOverlay;
        //input = player.input;
        bossGUIParent.gameObject.SetActive(false);
        pauseOverlay.CrossFadeAlpha(0.0f, 0.0f, true);
        pauseImage.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        score.text = GameManager.score + "\n<size=50><i>Score</i></size>";
        enemies.text = GameManager.enemyCount + "\n<size=50><i>Enem" + (GameManager.enemyCount == 1 ? "y" : "ies") + "</i></size>";
        float timeLeft = GameManager.endTime - Time.time;
        //timer.text = "<size=50><i>Time</i></size>\n" + (int)timeLeft;
        timerCircle.fillAmount = timeLeft / GameManager.gameDuration;
        playerShield.fillAmount = GameManager.boostTimer;
        playerShards.fillAmount = 1.0f * GameManager.curResources / GameManager.maxResources;
        playerSpeed.value = player.speed; //input.getSpeed() * (input.placingTurret() ? 0.5f : 1);
        playerHealth.fillAmount = playerHealthOverlay.fillAmount = player.percentOfHealth;
        baseHealth.fillAmount = baseHealthOverlay.fillAmount = spaceStation.percentOfHealth;
        bool paused = GameManager.gamePaused;
        if (paused != lastPaused)
        {
            lastPaused = paused;
            float targetAlpha = paused ? 1.0f : 0.0f;
            pauseOverlay.CrossFadeAlpha(targetAlpha, 1.0f, true);
            pauseImage.gameObject.SetActive(paused);
        }
    }
}
