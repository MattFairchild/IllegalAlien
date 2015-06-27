using UnityEngine;
using System.Collections;

public class SwitchGameObjectActive : MonoBehaviour {

	[SerializeField]protected GameObject[] objectsToSwitch;
	[SerializeField]protected bool objectsActive = false;

	void Start () {
		SwitchObjects(objectsActive);
	}

	public void SwitchObjects (bool active) {
		foreach(GameObject go in objectsToSwitch){
			go.SetActive(active);
		}
		objectsActive = active;
	}

	public void SwitchObjects () {
		SwitchObjects(!objectsActive);
	}
}
