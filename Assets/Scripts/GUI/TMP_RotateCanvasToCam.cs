using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TMP_RotateCanvasToCam : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		transform.rotation = Quaternion.Euler(90,0,0);
	}

}