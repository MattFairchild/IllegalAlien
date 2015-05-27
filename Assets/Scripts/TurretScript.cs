using UnityEngine;
using System.Collections;

public class TurretScript : MonoBehaviour
{
    public Rigidbody rb;
    public Vector3 direction;
    public float distance;
    public bool affectedByGravity = false;
    public GameObject[] planets;

    // Use this for initialization
    void Start()
    {
        planets = GameObject.FindGameObjectsWithTag("Planet");
        direction = Vector3.zero;
        rb.velocity = Vector3.zero;
    }

    void FixedUpdate()
    {
        if (affectedByGravity)
        {
            Gravity();
        }
    }

    public void SetVelocity(Vector3 vel)
    {
        affectedByGravity = true;
        rb.velocity = vel;
    }

    public void setAffectedByGravity(bool affected)
    {
        affectedByGravity = affected;
    }

    private void Gravity()
    {
        //temp object for the case that we only consider the nearest planet.
        //start it off as the first object if there are any panets at all
        GameObject nearestPlanet = this.gameObject;
        float tempDistance = 0.0f;
        if (planets.Length > 0)
        {
            nearestPlanet = planets[0];
            tempDistance = Vector3.Magnitude(nearestPlanet.transform.position - this.transform.position);
        }

        foreach (GameObject planet in planets)
        {
            //if we only want the nearest panet to attract, then use the loop only to find the nearest planet
            if (GameManager.getOnlyNearest() && planets.Length > 0)
            {
                if (Vector3.Magnitude(planet.transform.position - this.transform.position) < tempDistance)
                {
                    nearestPlanet = planet;
                    tempDistance = Vector3.Magnitude(planet.transform.position - this.transform.position);
                }
            }
            //otherwise calculate the gravity of every planet into the equation
            else
            { 
                Rigidbody planetRb = planet.GetComponent<Rigidbody>();
                direction = Vector3.Normalize(planet.transform.position - this.transform.position);
                distance = Vector3.Magnitude(planet.transform.position - this.transform.position);
                rb.velocity += ((GameManager.getGravitationalConstant() * planetRb.mass / Mathf.Pow(distance, 2)) * direction) * Time.deltaTime;       
            }
        }


        //if only the nearest planet should be used, calc same equation as above, but for the nearest planet only
        if (GameManager.getOnlyNearest() && planets.Length > 0)
        {
            Rigidbody planetRb = nearestPlanet.GetComponent<Rigidbody>();
            direction = Vector3.Normalize(nearestPlanet.transform.position - this.transform.position);
            distance = Vector3.Magnitude(nearestPlanet.transform.position - this.transform.position);
            rb.velocity += ((GameManager.getGravitationalConstant() * planetRb.mass / Mathf.Pow(distance, 2)) * direction) * Time.deltaTime;         
        }

    }

    // Collision with other objects
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Planet")
        {
			Destroy(this.gameObject);    
        }
        //TODO: Collision with other turrets
    }

}
