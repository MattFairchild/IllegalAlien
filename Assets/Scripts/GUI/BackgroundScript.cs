using UnityEngine;
using System.Collections;

public class BackgroundScript : MonoBehaviour {

	public bool fancyBackground = true;
	[SerializeField]protected Material mat1;
	[SerializeField]protected Material mat2;

	// Use this for initialization
	void Start () {
		GetComponent<Renderer>().material = fancyBackground ? mat1 : mat2;
	}
}
