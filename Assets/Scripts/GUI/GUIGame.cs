using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GUIGame : MonoBehaviour
{
    public bool AnimateUI;

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
    protected Image injureOverlay;
    [SerializeField]
    protected Image pauseOverlay;
    [SerializeField]
    protected RectTransform pauseImage;
    [SerializeField]
    protected Image winOverlay;
    [SerializeField]
    protected RectTransform winImage;
    [SerializeField]
    protected Image loseOverlay;
    [SerializeField]
    protected RectTransform loseImage;


    protected Player player;
    protected SpaceStationScript spaceStation;
    protected InputCapture input;
    protected Image playerHealthOverlay;
    protected Image baseHealthOverlay;

    protected bool lastPaused = false;

    private CanvasRenderer[] allCanvasRenderers;
    private const float GUITransitionDuration = 1;
    private const float GUIStartTransitionTime = 3.5f;

    // Use this for initialization
    void Start()
    {
        player = GameManager.player;
        spaceStation = GameManager.spaceStation;
        playerHealthOverlay = player.healthBarOverlay;
        baseHealthOverlay = spaceStation.healthBarOverlay;
        input = player.input;
        bossGUIParent.gameObject.SetActive(false);

        injureOverlay.CrossFadeAlpha(0.0f, 0.0f, true);
        injureOverlay.gameObject.SetActive(true);
        pauseOverlay.CrossFadeAlpha(0.0f, 0.0f, true);
        pauseOverlay.gameObject.SetActive(true);
        pauseImage.gameObject.SetActive(false);
        winOverlay.CrossFadeAlpha(0.0f, 0.0f, true);
        winOverlay.gameObject.SetActive(true);
        winImage.gameObject.SetActive(false);
        loseOverlay.CrossFadeAlpha(0.0f, 0.0f, true);
        loseOverlay.gameObject.SetActive(true);
        loseImage.gameObject.SetActive(false);

        if (AnimateUI)
        {
            allCanvasRenderers = this.GetComponentsInChildren<CanvasRenderer>();
            foreach (CanvasRenderer canvasRenderer in allCanvasRenderers)
            {
                canvasRenderer.SetAlpha(0);
            }
        }

        playerSpeed.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        score.text = GameManager.score + "\n<size=50><i>Score</i></size>";
        enemies.text = GameManager.enemyCount + "\n<size=50><i>Enem" + (GameManager.enemyCount == 1 ? "y" : "ies") + "</i></size>";
        float timeLeft = GameManager.endTime - Time.time;
        //timer.text = "<size=50><i>Time</i></size>\n" + (int)timeLeft;
        timer.text = "Time\n" + (int)timeLeft;
        timerCircle.fillAmount = timeLeft / GameManager.gameDuration;
        playerShield.fillAmount = GameManager.boostTimer;
        playerShards.fillAmount = 1.0f * GameManager.curResources / GameManager.maxResources - 0.01f;
        //playerSpeed.value = input.getSpeedNormalizedLength(); //player.speed; //input.getSpeed() * (input.placingTurret() ? 0.5f : 1);
        playerHealth.fillAmount = playerHealthOverlay.fillAmount = player.percentOfHealth;
        baseHealth.fillAmount = baseHealthOverlay.fillAmount = spaceStation.percentOfHealth;

        //set opacity of injure overlay to > 0 when either player or space station has less health than the lowHealthPercentage threshold
        float impulseMultipier = Mathf.Sin(5.666667f * Time.time) / 4f + 0.75f;
        injureOverlay.CrossFadeAlpha(Mathf.Clamp01(2 * Mathf.Max(spaceStation.lowHealthPercentage - spaceStation.percentOfHealth, player.lowHealthPercentage - player.percentOfHealth) * impulseMultipier), 0.1f, true);

        bool paused = GameManager.gamePaused;
        if (paused != lastPaused)
        {
            lastPaused = paused;
            float targetAlpha = paused ? 1.0f : 0.0f;
            pauseOverlay.CrossFadeAlpha(targetAlpha, 1.0f, true);
            pauseImage.gameObject.SetActive(paused);
        }

        //playerSpeed.gameObject.SetActive(input.placingTurret());

        float timeSinceStart = GameManager.TimeSinceStart;

        if (AnimateUI && GUIStartTransitionTime <= timeSinceStart && timeSinceStart < GUIStartTransitionTime + GUITransitionDuration)
        {
            float lerpFactor = Mathf.Clamp01((timeSinceStart - GUIStartTransitionTime) / GUITransitionDuration);
            foreach (CanvasRenderer canvasRenderer in allCanvasRenderers)
            {
                canvasRenderer.SetAlpha(lerpFactor);
            }

            injureOverlay.CrossFadeAlpha(0, 0, true);
            pauseOverlay.CrossFadeAlpha(0, 0, true);
            loseOverlay.CrossFadeAlpha(0, 0, true);
            winOverlay.CrossFadeAlpha(0, 0, true);
        }
    }

    public void ShowGameOverOverlay(bool won)
    {
        if (won)
        {
            winOverlay.CrossFadeAlpha(1.0f, 1.0f, true);
            winImage.gameObject.SetActive(true);
        }
        else
        {
            loseOverlay.CrossFadeAlpha(1.0f, 1.0f, true);
            loseImage.gameObject.SetActive(true);
        }
    }
}
