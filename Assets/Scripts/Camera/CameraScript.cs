using UnityEngine;
using System.Collections;

public class CameraScript : MonoBehaviour {
	
	public float xBounds = 30.0f;
	public float zBounds = 20.0f;
	public float yDistance = 20.0f;
	
	private Transform player;
	private Camera cam;
	// Use this for initialization
	void Start () {
		player = GameObject.FindGameObjectWithTag("Player").transform;
		this.transform.position = new Vector3(player.position.x, yDistance, player.position.z);
		cam = GetComponent<Camera>();
	}
	
	// Update is called once per frame
	void Update () {
		float correctX, correctZ, correctY = 20.0f;
		bool correct = false;
		this.transform.position = new Vector3(player.position.x, this.transform.position.y, player.position.z);
		
		if (transform.position.x < -xBounds)
		{
			correctX = -xBounds;
			correct = true;
		}
		else if (transform.position.x > xBounds)
		{
			correctX = xBounds;
			correct = true;
		}
		else 
		{
			correctX = transform.position.x;
		}
		
		
		if (transform.position.z < -zBounds)
		{
			correctZ = -zBounds;
			correct = true;
		}
		else if (transform.position.z > zBounds)
		{
			correctZ = zBounds;
			correct = true;
		}
		else
		{
			correctZ = transform.position.z;
		}
		
		
		if (correct)
		{
			this.transform.position = new Vector3(correctX, correctY, correctZ);
		}
		
	}
}
