using UnityEngine;
using System.Collections;

public class RotateCamera : MonoBehaviour {

	[SerializeField] protected Transform cameraObj;
	[SerializeField] protected float rotationSpeed = 10;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		cameraObj.Rotate(Vector3.up, rotationSpeed * (1 + 0.5f * Mathf.Sin(0.25f * Time.time)) * Time.deltaTime);
	}
}
