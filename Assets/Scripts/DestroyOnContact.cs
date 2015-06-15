using UnityEngine;
using System.Collections;

public class DestroyOnContact : MonoBehaviour {

	void OnTriggerEnter (Collider other) {
		Destroy(other.gameObject);
	}
}
