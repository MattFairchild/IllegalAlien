﻿using UnityEngine;
using System.Collections;

public class s_LevelBorders : MonoBehaviour {

	[SerializeField]protected float borderCubeRadius = 65;
	[SerializeField]protected float borderThickness = 2;

	void Awake () {
		foreach(Transform child in transform){
			Vector3 newPos = child.localPosition *= borderCubeRadius;
			Vector3 newScale = child.localScale * borderCubeRadius;
			for(int i = 0; i < 3; i++){
				newPos[i] += newScale[i] == 0 ? Mathf.Sign(newPos[i]) * borderThickness/2 : 0;
				newScale[i] = newScale[i] == 0 ? borderThickness : newScale[i] + 2*borderThickness;
			}
			child.localPosition = newPos;
			child.localScale = newScale;
		}
	}

	void OnDrawGizmos () {
		Color regColor = Gizmos.color;
		Gizmos.color = Color.white;
		
		Gizmos.DrawWireCube(transform.position, 2*borderCubeRadius * Vector3.one);
		Gizmos.DrawWireCube(transform.position, 2*(borderThickness + borderCubeRadius) * Vector3.one);

		Gizmos.color = regColor;
	}
}
