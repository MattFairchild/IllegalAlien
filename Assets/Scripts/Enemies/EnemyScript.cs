using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public abstract class EnemyScript : MonoBehaviour 
{

    [SerializeField]protected float maxHealth;
    protected float curHealth;
	[SerializeField]protected int resources;
	[SerializeField]protected GameObject scrapPrefab;
    protected new Rigidbody rigidbody;
	[SerializeField]protected Image healthBar;

	protected GameObject spaceStation;
	protected GameObject player;
	protected GameObject target;

	protected void InitializeEnemy () {
		player = GameObject.FindGameObjectWithTag("Player");
		spaceStation = GameObject.FindGameObjectWithTag("SpaceStation");
		
		rigidbody = gameObject.GetComponent<Rigidbody>();
		curHealth = maxHealth;
	}

	public void Hit (float damage) {
		curHealth -= damage;
		healthBar.fillAmount = curHealth/maxHealth;
		//Color change?
		if (curHealth <= 0)	{
			Die ();
		}
	}

	protected void Die () {
		GameObject scrap = (GameObject)Instantiate(scrapPrefab, transform.position, Quaternion.identity);
		ResourcesScript rs = scrap.GetComponent<ResourcesScript>();
		rs.resources = resources;

		Rigidbody rbE = gameObject.GetComponent<Rigidbody>();
		Rigidbody rbS = rs.GetComponent<Rigidbody>();
		rbS.velocity = rbE.velocity;
		rbS.angularVelocity = rbE.angularVelocity;

		Destroy(gameObject);
	}

    void OnCollisionEnter(Collision collision)
    {
        switch (collision.gameObject.tag){
		case "Bullet":
			Hit (collision.gameObject.GetComponent<BulletScript>().damage);
			break;
		case "Planet":
			Hit (0.25f * collision.relativeVelocity.magnitude);
			break;
		case "Player":
			Hit (0.2f * collision.relativeVelocity.magnitude);
			break;
		case "Turret":
			Hit (0.2f * collision.relativeVelocity.magnitude);
			break;
		case "Enemy":
			Hit (0.1f * collision.relativeVelocity.magnitude);
			break;
		default:
			break;
        }
    }
}
