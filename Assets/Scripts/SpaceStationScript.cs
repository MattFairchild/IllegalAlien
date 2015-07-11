using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SpaceStationScript : Agent, IHittable
{

    public Image healthBarOverlay;

    public MaterialColorController MaterialControllerForLowHealth;
    private Gradient normalGradient;
    private Gradient warningGradient;

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

    void OnCollisionEnter(Collision collision)
    {
        InstantiateCollisionEffect(collision.contacts[0].point);
    }

    protected void Die()
    {
        alive = false;

        Instantiate(deathExplosionPrefab, transform.position, Quaternion.identity);
        Instantiate(deathSoundPrefab, transform.position, Quaternion.identity);
        //gameObject.GetComponent<AudioSource>().PlayOneShot(deathClip, 2.0f);
        this.transform.localScale = Vector3.zero;


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
    }

    public override void IncreaseKillCount()
    {
        killCount++;
    }
}
