using UnityEngine;
using System.Collections;

public class InterceptorScript : EnemyScript
{
    public bool attackTurrets;
    public bool attackPlayer;
    private bool targetInRange;

    // Use this for initialization
    void Start()
    {
        InitializeEnemy();
        targetInRange = false;
        target = player;
        StartCoroutine(Fight());
    }

    IEnumerator Fight()
    {
        while (true)
        {
            if (attackTurrets)
            {
                GameManager.turretList.RemoveAll(item => item == null);
                foreach (GameObject turret in GameManager.turretList)
                {
                    if (Vector3.Distance(turret.transform.position, transform.position) <= shootingRange && Vector3.Angle(transform.forward, turret.transform.position - transform.position) < 15f)
                    {
                        targetInRange = true;
                    }
                }
            }
            if (attackPlayer && Vector3.Distance(player.transform.position, transform.position) <= shootingRange && Vector3.Angle(transform.forward, player.transform.position - transform.position) < 15f)
            {
                targetInRange = true;      
            }

            if (targetInRange)
            {
                Shoot();
                targetInRange = false;
                yield return new WaitForSeconds(1 / shootingFrequency);
            }
            else
            {
                yield return new WaitForSeconds(0.1f);
            }
        }
    }

    void Shoot()
    {
        Vector3 position = transform.TransformPoint(new Vector3(0.4f, 0, 2.0f));

        BulletScript bs1 = (GameObject.Instantiate(bulletPrefab, position, Quaternion.LookRotation(transform.forward)) as GameObject).GetComponent<BulletScript>();
        bs1.sender = this;
        bs1.damage = damagePerShot;
        bs1.speed = projectileSpeed;

        position = transform.TransformPoint(new Vector3(-0.4f, 0, 2.0f));
        BulletScript bs2 = (GameObject.Instantiate(bulletPrefab, position, Quaternion.LookRotation(transform.forward)) as GameObject).GetComponent<BulletScript>();
        bs2.sender = this;
        bs2.damage = damagePerShot;
        bs2.speed = projectileSpeed;

        audio[0].PlayOneShot(audio[0].clip);
    }
}
