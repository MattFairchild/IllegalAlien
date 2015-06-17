using UnityEngine;
using System.Collections;

public interface IHittable {
	void Hit (float damage, Agent attacker = null);
}

public abstract class Agent : MonoBehaviour {
	[SerializeField]protected float maxHealth;
	protected float curHealth;
    [SerializeField]protected float lowHealthPercentage = 0.2f;
	
	public float percentOfHealth;
	protected int killCount = 0;

	protected void InitializeAgent () {
		curHealth = maxHealth;
		percentOfHealth = 1.0f;
	}

	public abstract void IncreaseKillCount();
}