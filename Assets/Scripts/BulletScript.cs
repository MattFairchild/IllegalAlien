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
		rb.MoveRotation(Quaternion.LookRotation(dir));
		SetVelocity(dir * speed);
		//rb.velocity += dir * Time.fixedDeltaTime - rb.velocity.normalized * Time.fixedDeltaTime;
	}

    public void SetVelocity(Vector3 vel)
    {
        rb.velocity = vel;
    }

    protected void OnCollisionEnter(Collision collision) {
		switch(faction){
		case GameManager.Factions.Player:
			//enemy faction member hit
	        if(GameManager.FactionTagsEnemy.Contains(collision.gameObject.tag)){
				collision.gameObject.GetComponent<EnemyScript>().Hit(damage);
				//Destroy(gameObject);
			}/*
			else if (collision.gameObject.tag != "Player" && collision.collider.tag != "Gun"){
	            Destroy(gameObject);
	        }*/
			Destroy(gameObject);
			break;
		case GameManager.Factions.Enemy:
			if(GameManager.FactionTagsPlayer.Contains(collision.gameObject.tag)){
				collision.gameObject.GetComponent<IHittable>().Hit(damage);
			}
			//else if(collision.gameObject.tag != "Enemy"){
			Destroy(gameObject);
			break;
		case GameManager.Factions.None:
			Destroy(gameObject);
			break;
		}
    }
}
