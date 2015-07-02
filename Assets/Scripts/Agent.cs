using UnityEngine;
using System.Collections;

public interface IHittable {
	void Hit (float damage, Agent attacker = null);
}

public abstract class Agent : MonoBehaviour {
	[SerializeField]protected float maxHealth;
	protected float curHealth;
	protected bool alive = true;

    public float lowHealthPercentage = 0.2f;
	[SerializeField]protected GameObject collisionEffectPrefab;
	[SerializeField]protected GameObject deathExplosionPrefab;
	[SerializeField]protected GameObject deathSoundPrefab;
	
	public float percentOfHealth;
	protected int killCount = 0;

	protected void InitializeAgent () {
		curHealth = maxHealth;
		percentOfHealth = 1.0f;
	}

	protected void InstantiateCollisionEffect (Vector3 collisionContactPos) {
		if(collisionEffectPrefab){
			(Instantiate(collisionEffectPrefab, Vector3.Lerp(collisionContactPos, transform.position, 0.1f) + 0.15f * Vector3.up, Quaternion.identity)as GameObject).transform.SetParent(transform);
		}
	}

	public abstract void IncreaseKillCount();
}