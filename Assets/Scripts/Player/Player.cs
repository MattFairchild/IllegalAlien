using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Player : Agent, IHittable
{

    //[SerializeField]protected int maxResources;

    //[SerializeField]protected Image healthBar;
    //[SerializeField]protected Image shieldBar;
    //[SerializeField]protected Image shardsBar;
    //[SerializeField]protected Slider speedBar;
    public Image healthBarOverlay;
    public InputCapture input;

    //protected Vector3 lastPos = Vector3.zero;
    public float speed;

    public MaterialColorController MaterialControllerForLowHealth;
    private Gradient normalGradient;
    private Gradient warningGradient;

    [SerializeField]
    protected GameObject lowHealthWarningPrefab;

    // Use this for initialization
    void Start()
    {
        InitializeAgent();

        normalGradient = new Gradient();
        normalGradient.colorKeys = (GradientColorKey[])MaterialControllerForLowHealth.Colors.colorKeys.Clone();

        warningGradient = new Gradient();
        warningGradient.colorKeys = new[] { new GradientColorKey(new Color(0.5f, 0, 0), 0), new GradientColorKey(new Color(1, 0, 0), 1) };

    }

    // Update is called once per frame
    void Update()
    {
        //healthBarOverlay.fillAmount = percentOfHealth;
        //healthBar.fillAmount = percentOfHealth;
        //shardsBar.fillAmount = (float)GameManager.curResources / (float)maxResources;
        //moved to GUI script!
        //speedBar.value = speed;
        //speed = 0.1f * (transform.position - lastPos).magnitude / Time.fixedDeltaTime;
        //lastPos = transform.position;
        if (input.placingTurret())
        {
            speed = 0.5f * input.getSpeedNormalizedLength();
        }
        else
        {
            speed = 1.0f * input.getSpeedNormalizedLength();
        }

        if (percentOfHealth > lowHealthPercentage)
        {
            MaterialControllerForLowHealth.Colors.colorKeys = normalGradient.colorKeys;
            MaterialControllerForLowHealth.Frequence = 2.833333f;
        }
        else
        {
            float dramatist = (1 - (percentOfHealth - 0.1f) / lowHealthPercentage);

            GradientColorKey startColorKey =
                new GradientColorKey(Color.Lerp(normalGradient.Evaluate(0), warningGradient.Evaluate(0), dramatist), 0);
            GradientColorKey endColorKey =
                new GradientColorKey(Color.Lerp(normalGradient.Evaluate(1), warningGradient.Evaluate(1), dramatist), 1);

            MaterialControllerForLowHealth.Colors.colorKeys = new[] { startColorKey, endColorKey };
            MaterialControllerForLowHealth.Frequence = 5.666667f;
        }
    }

    protected void Die()
    {
        alive = false;

        Instantiate(deathExplosionPrefab, transform.position, Quaternion.identity);
        Instantiate(deathSoundPrefab, transform.position, Quaternion.identity);
        //gameObject.GetComponentInChildren<AudioSource>().PlayOneShot(deathClip, 2.0f);

        Debug.Log("Game Over!");
        GameManager.GameOver();
    }

    public void Hit(float damage, Agent attacker = null)
    {
        if (!alive)
        {
            return;
        }

        curHealth -= damage;
        percentOfHealth = curHealth / maxHealth;
        if (curHealth <= 0)
        {
            Die();
            if (attacker)
            {
                attacker.IncreaseKillCount();
            }
        }
        else if (percentOfHealth < lowHealthPercentage)
        {
            //GameObject warning = (Instantiate(lowHealthWarningPrefab, transform.position + 0.5f*Vector3.up, Quaternion.identity) as GameObject);
            //warning.transform.SetParent(transform);
            //warning.GetComponent<UnityStandardAssets.Effects.ParticleSystemMultiplier>().multiplier = 1.5f;
        }
    }

    public override void IncreaseKillCount()
    {
        killCount++;
    }

    void OnCollisionEnter(Collision collision)
    {
        InstantiateCollisionEffect(collision.contacts[0].point);
        switch (collision.gameObject.tag)
        {
            case "Enemy":
                Hit(0.2f * collision.relativeVelocity.magnitude);
                break;
            case "Planet":
                //Hit (0.5f * collision.relativeVelocity.magnitude);
                //Debug.Log("blub" + collision.relativeVelocity.magnitude);
                Hit(0.5f * input.getSpeedNormalizedLength() * (input.placingTurret() ? 2.5f : 5.0f));
                break;
            default:
                break;
        }
    }

    public Vector3 getCurrentVelocity()
    {
        return (transform.forward * input.getSpeedNormalizedLength() * GetComponent<InputActions>().maxSpeed);
    }

    public float maxSpeed
    {
        get { return GetComponent<InputActions>().maxSpeed; }
    }
}
