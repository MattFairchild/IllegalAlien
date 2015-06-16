using UnityEngine;
using System.Collections;

public class s_LevelBorders : MonoBehaviour {

	[SerializeField]protected float borderCubeRadius = 65;
	[SerializeField]protected float borderThickness = 2;

	void Awake () {
		foreach(Transform child in transform){
			child.position *= borderCubeRadius;
			Vector3 newScale = child.localScale * borderCubeRadius;
			for(int i = 0; i < 3; i++){
				newScale[i] = newScale[i] == 0 ? borderThickness : newScale[i];
			}
			child.localScale = newScale;
		}
	}

}
