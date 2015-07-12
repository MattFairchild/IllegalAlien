using UnityEngine;
using System.Collections;

public class s_MinimapDots : MonoBehaviour {

	//[SerializeField]protected GameObject dotSpherePrefab;
	//[SerializeField]protected Color dotColor;
	//[SerializeField]protected float dotScale;
	[SerializeField]protected float dotSphereYoffset = 40;


	// Use this for initialization
	void Start () {
		GetComponent<Renderer>().enabled = true;
		Vector3 pos = transform.position;
		pos.y = dotSphereYoffset;
		transform.position = pos;
	}
}
