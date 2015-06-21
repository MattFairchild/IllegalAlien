using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DestroyerScript : EnemyScript
{
    [SerializeField]protected GameObject torpedoPrefab;
    [SerializeField]protected float damagePerTorpedo = 10f;
    [SerializeField]protected float torpedoShootingFrequency = 0.25f;
    [SerializeField]protected float torpedoShootingRange = 15.0f;
    [SerializeField]protected float torpedoSpeed = 10.0f;
    [SerializeField]protected SphereCollider trigger;

    private List<GameObject> enemiesInRange = new List<GameObject>();

	// Use this for initialization
	void Start ()
    {
		InitializeEnemy();
		target = spaceStation;
        trigger.radius = shootingRange;
        StartCoroutine(Fight());
        StartCoroutine(FightSpaceStation());
	}
	
	// Update is called once per frame
	void Update () 
    {
        agent.destination = spaceStation.transform.position;
	}

    IEnumerator Fight()
    {
        while (true)
        {
			enemiesInRange.RemoveAll(item => item == null);
            if (enemiesInRange.Count > 0)
            {
                GameObject pickedTarget = PickTarget();
                ShootProjectile(pickedTarget);
                yield return new WaitForSeconds(1f / shootingFrequency);
            }
            else
            {
                yield return new WaitForSeconds(0.1f);
            }
        }
    }

    IEnumerator FightSpaceStation()
    {
        while (true)
        {
            if (Vector3.Distance(transform.position, spaceStation.transform.position) <= torpedoShootingRange && Vector3.Angle(transform.forward, spaceStation.transform.position - transform.position) < 15f)
            {
                ShootTorpedo();
                yield return new WaitForSeconds(1f / torpedoShootingFrequency);
            }
            else
            {
                yield return new WaitForSeconds(0.1f);
            }
        }
    }

    GameObject PickTarget()
    {
        GameObject pickedEnemy = null;
        float distance = 100f;
		for (int i = 0; i < enemiesInRange.Count; i++ ) {
			//this code doesn't make sense!!
            /*if (enemiesInRange[i] == null)
            {
                enemiesInRange.Remove(enemiesInRange[i]);
            }*/
            if (Vector3.Distance(transform.position, enemiesInRange[i].transform.position) < distance)
            {
                distance = Vector3.Distance(transform.position, enemiesInRange[i].transform.position);
                pickedEnemy = enemiesInRange[i];
            }
        }
        return pickedEnemy;
    }

    void ShootProjectile(GameObject pickedTarget)
    {
        Vector3 direction = transform.position + (Vector3.Normalize(pickedTarget.transform.position - transform.position) * 2.3f);
        BulletScript bs = (GameObject.Instantiate(bulletPrefab, direction, Quaternion.LookRotation(direction - transform.position)) as GameObject).GetComponent<BulletScript>();
        bs.sender = this;
        bs.damage = damagePerShot;
        bs.speed = projectileSpeed;
        audio[0].PlayOneShot(audio[0].clip);
    }

    void ShootTorpedo()
    {
        Vector3 position = transform.TransformPoint(new Vector3(0, 0, 2.7f));
        BulletScript bs = (GameObject.Instantiate(bulletPrefab, position, Quaternion.LookRotation(transform.forward)) as GameObject).GetComponent<BulletScript>();
        bs.sender = this;
        bs.damage = damagePerTorpedo;
        bs.speed = torpedoSpeed;
        audio[1].PlayOneShot(audio[1].clip);
    }

    void OnTriggerEnter (Collider other) {
		if (other.tag == "Player" || other.tag == "Turret")
        {
            enemiesInRange.Add(other.gameObject);
		}
	}

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player" || other.tag == "Turret")
        {
            enemiesInRange.Remove(other.gameObject);
        }
    }
    
}
