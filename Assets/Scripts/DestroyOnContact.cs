using UnityEngine;
using System.Collections;

public class DestroyOnContact : MonoBehaviour {

	public float lastTimeOfPlayerEnter;

	void OnTriggerEnter (Collider other) {
		if(other.tag.Equals("Player")){
			lastTimeOfPlayerEnter = Time.time;
		}
		else{
			Destroy(other.gameObject);
		}
	}

	void OnTriggerStay (Collider other) {
		if(other.tag.Equals("Player")){
			GameManager.player.Hit(Time.deltaTime * 5.0f * (Time.time - lastTimeOfPlayerEnter)); //increase DPS on player the longer he stays
		}
	}
}
