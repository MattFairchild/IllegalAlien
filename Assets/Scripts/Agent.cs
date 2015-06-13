using UnityEngine;
using System.Collections;

public interface IHittable {
	void Hit (float damage);
}

public abstract class Agent : MonoBehaviour {
	[SerializeField]protected float maxHealth;
	protected float curHealth;
	
	public float percentOfHealth;

	protected void InitializeAgent () {
		curHealth = maxHealth;
		percentOfHealth = 1.0f;
	}
}