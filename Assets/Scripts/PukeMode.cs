using UnityEngine;
using System.Collections;

public class PukeMode : MonoBehaviour {

	protected Renderer rend;
	void Start() {
		rend = GetComponent<Renderer>();
	}
	void Update() {
		float scaleX = Mathf.Cos(Time.time) * 0.75F + 2;
		float scaleY = Mathf.Sin(Time.time) * 0.75F + 2;
		rend.material.mainTextureScale = new Vector2(scaleX, scaleY);
	}
}
