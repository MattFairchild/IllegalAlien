using UnityEngine;
using System.Collections;
using System.Collections.Generic;
//using System;

public class KDMapGenerator : MonoBehaviour {
	
	
	public GameObject testPrefab;
	public GameObject tempPrefab;
	private GameObject tempObject;
	
	private Rect area;
	private List<Rect> rects = new List<Rect>();
	private int indexLargest;
	
	public bool showBoxCorners;
	public float distanceToOrigin = 15.0f;
	public float offsetRange = 0;
	public bool drawGizmos = false;

	void OnDrawGizmos () {
		if(drawGizmos){
			Color regColor = Gizmos.color;
			Gizmos.color = Color.green;

			foreach(Rect rect in rects){
				//Vector3 pos = new Vector3(rect.position.x, 0, rect.position.y);
				Vector3 center = new Vector3(rect.center.x, 0, rect.center.y);
				Vector3 size = new Vector3(rect.size.x, 0, rect.size.y);
				Gizmos.DrawWireCube(center, size);
			}
			
			Gizmos.color = regColor;
		}
	}

	// Use this for initialization
	void Start () {
		CameraScript cam = Camera.main.GetComponent<CameraScript>(); //GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraScript>();
		area.xMin = -cam.xBounds*1.5f;
		area.yMin = cam.zBounds * 1.5f;
		area.xMax = cam.xBounds * 1.5f;
		area.yMax = -cam.zBounds * 1.5f;
		rects.Add(area);
		
		indexLargest = 0;
		
		int numPLanets = UnityEngine.Random.Range(6, 9);
		for (int i = 0; i < numPLanets; i++)
		{
			createPlanet();
		}
		
		placePlanets();
		
		if (showBoxCorners)
		{
			Vector3 pos;
			foreach (Rect r in rects)
			{
				pos = new Vector3(r.x, 0.0f, r.y);
				Instantiate(testPrefab, pos, Quaternion.identity);
				
				pos = new Vector3(r.x + r.width, 0.0f, r.y);
				Instantiate(testPrefab, pos, Quaternion.identity);
				
				pos = new Vector3(r.x + r.width, 0.0f, r.y + r.height);
				Instantiate(testPrefab, pos, Quaternion.identity);
				
				pos = new Vector3(r.x, 0.0f, r.y + r.height);
				Instantiate(testPrefab, pos, Quaternion.identity);
			}
		}
		
	}
	
	
	/*
    / goes through all the rects and sets a planet in the center
    / it then rescales the planet accodring to the smaller of either the rects width or height
    */    
	void placePlanets()
	{
		Vector3 pos;
		
		foreach (Rect r in rects)
		{
            float radius = Mathf.Min(Mathf.Abs(r.width), Mathf.Abs(r.height)) / 2.0f;
            float mass = 1.5f * (Mathf.Pow(1.5f, 2.0f) * radius) / GameManager.getGravitationalConstant();
			float newSize = UnityEngine.Random.Range(mass, mass*1.2f);
			
			
			pos = getPlanetPosition(r, radius);
			Vector2 offset = Random.insideUnitCircle;
			pos += offsetRange * new Vector3(offset.x, 0, offset.y);
			tempObject = (GameObject)Instantiate(tempPrefab, pos, Quaternion.identity);
			tempObject.GetComponent<PlanetScript>().scaleTo(newSize);
		}
	}
	
	private void createPlanet()
	{
		splitRect(getLargestRect(rects));
	}
	
	// check if the width or height is larger and split along the larger side
	void splitRect(Rect rec)
	{
		if (Mathf.Abs(rec.width) > Mathf.Abs(rec.height))
		{
			splitVertically(rec);
		}
		else
		{
			splitHorizontally(rec);
		}
	}
	
	
	/*
    / split functions take a rect and split it at a random point between 0.25 and 0.75 of the length
    / it then removes the currently largest rect, which should be the one that was just splitted,
    / and adds the 2 new rects
    */
	private void splitVertically(Rect rec)
	{
		float position = UnityEngine.Random.Range(rec.width*0.25f, rec.width*0.75f);
		
		Rect rec1 = new Rect();
		rec1.x = rec.x;
		rec1.y = rec.y;
		rec1.width = position;
		rec1.height = rec.height;
		
		Rect rec2 = new Rect();
		rec2.x = rec.x + position;
		rec2.y = rec.y;
		rec2.width = rec.width - position;
		rec2.height = rec.height;
		
		rects.RemoveAt(indexLargest);
		rects.Add(rec1);
		rects.Add(rec2);
	}
	
	private void splitHorizontally(Rect rec)
	{
		float position = UnityEngine.Random.Range(rec.height * 0.25f, rec.height * 0.75f);
		
		Rect rec1 = new Rect();
		rec1.x = rec.x;
		rec1.y = rec.y;
		rec1.width = rec.width;
		rec1.height = position;
		
		Rect rec2 = new Rect();
		rec2.x = rec.x;
		rec2.y = rec.y + position;
		rec2.width = rec.width;
		rec2.height = rec.height - position;
		
		rects.RemoveAt(indexLargest);
		rects.Add(rec1);
		rects.Add(rec2);
	}
	
	//returns the largest rect in the list, and sets the variable to the largest rect's index in the list
	Rect getLargestRect(List<Rect> list)
	{
		Rect temp = list[0];
		int counter = 0;
		indexLargest = 0;
		
		foreach (Rect r in list)
		{
			if (getLargerRect(temp, r) == 1)
			{
				temp = r;
				indexLargest = counter;
			};
			
			counter++;
		}
		
		return temp;
	}
	
	//return 0 if the first parameter was the larger rect, 1 f the second one was larger
	int getLargerRect(Rect rec1, Rect rec2)
	{
		float area1 = Mathf.Abs(rec1.width) * Mathf.Abs(rec1.height);
		float area2 = Mathf.Abs(rec2.width) * Mathf.Abs(rec2.height);
		
		if (area1 >= area2)
		{
			return 0;
		}
		else
		{
			return 1;
		}
	}
	
	//return the position the planet should be placed along the center of the larger side of the rect somwhere
	//with at least a distance of radius to the edge
	Vector3 getPlanetPosition(Rect rec, float radius)
	{
		Vector3 pos = new Vector3(rec.center.x, 0.0f, rec.center.y); ;
		int counter = 0;
		
		while (Vector3.Distance(pos, Vector3.zero) < distanceToOrigin && counter < 6)
		{
			if (rec.width > rec.height)
			{
				float x = UnityEngine.Random.Range(rec.xMin + radius, rec.xMax - radius);
				pos = new Vector3(x, 0.0f, rec.center.y);
			}
			else
			{
				float y = UnityEngine.Random.Range(rec.yMin - radius, rec.yMax + radius); //has to be yMin MINUS/yMax PLUS, since the min starts at the top in positive range and max is in negative
				pos = new Vector3(rec.center.x, 0.0f, y);
			}
			
			counter++;
		}
		return pos;
	}
}
