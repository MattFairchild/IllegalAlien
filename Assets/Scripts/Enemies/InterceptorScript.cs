using UnityEngine;
using System.Collections;

public class InterceptorScript : EnemyScript
{

    // Use this for initialization
    void Start()
    {
        InitializeEnemy();
        target = player;
        StartCoroutine(Fight());
    }

    // Update is called once per frame
    void Update()
    {
        agent.destination = target.transform.position;
    }

    IEnumerator Fight()
    {
        while (true)
        {

            if (Vector3.Distance(player.transform.position, transform.position) <= shootingRange && Vector3.Angle(transform.forward, player.transform.position - transform.position) < 15f)
            {
                Shoot();
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
        Vector3 position = transform.TransformPoint(new Vector3(0.7f, 0, 2.0f));

        BulletScript bs1 = (GameObject.Instantiate(bulletPrefab, position, Quaternion.LookRotation(transform.forward)) as GameObject).GetComponent<BulletScript>();
        bs1.sender = this;
        bs1.damage = damagePerShot;
        bs1.speed = projectileSpeed;

        position = transform.TransformPoint(new Vector3(-0.7f, 0, 2.0f));
        BulletScript bs2 = (GameObject.Instantiate(bulletPrefab, position, Quaternion.LookRotation(transform.forward)) as GameObject).GetComponent<BulletScript>();
        bs2.sender = this;
        bs2.damage = damagePerShot;
        bs2.speed = projectileSpeed;

        audio[0].PlayOneShot(audio[0].clip);
    }
}
