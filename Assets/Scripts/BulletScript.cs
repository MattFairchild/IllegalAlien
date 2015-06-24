using UnityEngine;
using System.Collections;
using System.Linq;

public class BulletScript : MonoBehaviour {

    public Rigidbody rb;
    public float speed = 25.0f;
    public float lifetime = 4.0f;
    public float damage = 1;
	public GameManager.Factions faction = GameManager.Factions.Player;
	public Transform target = null;
	public bool targetSeeking = false;
	public bool areaOfEffect = false;
	public float aoeRadius = 0;
	public float aoeDamageModifier = 0;
	public Agent sender = null;
	[SerializeField]protected GameObject explosionPrefab;

	// Use this for initialization
	void Start () {
        this.transform.rotation = Quaternion.AngleAxis(90.0f, this.transform.right);
        SetVelocity(this.transform.up * speed);
        //lifetime = 4.0f;
	}

    public void Update () {
        lifetime -= Time.deltaTime;

        if (lifetime < 0.0f)
        {
            GameObject.Destroy(this.gameObject);
        }
    }

	public void FixedUpdate () {
		if(!targetSeeking || !target){
			return;
		}
		Vector3 dir = (target.position - transform.position).normalized;
		Quaternion rot = Quaternion.LookRotation(dir);
        Quaternion x90 = Quaternion.AngleAxis(90, Vector3.right);
        rb.MoveRotation(rot * x90); //default rotation for capsule is facing up, not forward as desired for projectiles
		SetVelocity(dir * speed);
		//rb.velocity += dir * Time.fixedDeltaTime - rb.velocity.normalized * Time.fixedDeltaTime;
	}

    public void SetVelocity(Vector3 vel)
    {
        rb.velocity = vel;
    }

    protected void OnCollisionEnter(Collision collision) {
		bool explode = true;
		switch(faction){
		case GameManager.Factions.Player:
			//enemy faction member hit
	        if(GameManager.FactionTagsEnemy.Contains(collision.gameObject.tag)){
				collision.gameObject.GetComponent<EnemyScript>().Hit(damage, sender);
				if(areaOfEffect){
					PerformAreaOfEffect(collision.gameObject);
				}
				//Destroy(gameObject);
			}
			else if(GameManager.FactionTagsPlayer.Contains(collision.gameObject.tag)){
				explode = false;
			}
			/*
			else if (collision.gameObject.tag != "Player" && collision.collider.tag != "Gun"){
	            Destroy(gameObject);
	        }*/
			Destroy(gameObject);
			break;
		case GameManager.Factions.Enemy:
			if(GameManager.FactionTagsPlayer.Contains(collision.gameObject.tag)){
				collision.gameObject.GetComponent<IHittable>().Hit(damage, sender);
			}
			else if(GameManager.FactionTagsEnemy.Contains(collision.gameObject.tag)){
				explode = false;
			}
			//else if(collision.gameObject.tag != "Enemy"){
			Destroy(gameObject);
			break;
		case GameManager.Factions.None:
			Destroy(gameObject);
			break;
		}
		if(explosionPrefab && explode){
			Instantiate(explosionPrefab, collision.contacts[0].point, Quaternion.identity);
		}
    }

	protected void PerformAreaOfEffect (GameObject obj) {
		Collider[] colliders = Physics.OverlapSphere(obj.transform.position, aoeRadius);
		foreach(Collider col in colliders){
			if(GameManager.FactionTagsEnemy.Contains(col.tag)){
				col.GetComponent<EnemyScript>().Hit(aoeDamageModifier * damage, sender);
			}
		}
	}
}
