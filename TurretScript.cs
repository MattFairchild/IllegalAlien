using UnityEngine;
using System.Collections;

public class TurretScript : MonoBehaviour {

    public Vector3 velocity;
    public Vector3 direction;
    public float distance;
    public float speed;
    public float gravitationalConstant;
    public GameObject[] planets;

	// Use this for initialization
	void Start () 
    {
        planets = GameObject.FindGameObjectsWithTag("Planet");
        direction = Vector3.zero;
        velocity = Vector3.zero;
        gravitationalConstant = 0.3f;

	}
	

	// Update is called once per frame
	void Update () 
    {

	}

    void FixedUpdate()
    {
        transform.Translate(velocity * Time.deltaTime);
        Gravity();
        speed = Vector3.Magnitude(velocity);
    }

    public void SetVelocity(Vector3 vel)
    {
        velocity = vel;
    }

    private void Gravity()
    {
        foreach (GameObject planet in planets)
        {
            PlanetScript planetScript = planet.GetComponent<PlanetScript>();
            direction = Vector3.Normalize(planet.transform.position - this.transform.position);
            distance = Vector3.Magnitude(planet.transform.position - this.transform.position);
            velocity += (gravitationalConstant * planetScript.mass / Mathf.Pow(distance, 2)) * direction;

        }
    }

    // Collision with other objects
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Planet")
        {
            Vector3 mov = Vector3.Reflect(velocity, collision.contacts[0].normal);
            velocity = mov;

        }
    }
}
