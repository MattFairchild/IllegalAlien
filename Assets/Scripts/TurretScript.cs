using UnityEngine;
using System.Collections;

public class TurretScript : MonoBehaviour
{
    public Rigidbody rb;

    public Vector3 velocity;
    public Vector3 direction;
    public float distance;
    public float speed;
    public float gravitationalConstant;
    public bool affectedByGravity = false;
    public GameObject[] planets;

    // Use this for initialization
    void Start()
    {
        planets = GameObject.FindGameObjectsWithTag("Planet");
        direction = Vector3.zero;
        rb.velocity = Vector3.zero;
        gravitationalConstant = 30f;
    }

    void FixedUpdate()
    {
        //transform.Translate(velocity * Time.deltaTime);
        if (affectedByGravity)
        {
            Gravity();
        }
        speed = Vector3.Magnitude(rb.velocity);
        velocity = rb.velocity;
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
        foreach (GameObject planet in planets)
        {
            Rigidbody planetRb = planet.GetComponent<Rigidbody>();
            //PlanetScript planetScript = planet.GetComponent<PlanetScript>();
            direction = Vector3.Normalize(planet.transform.position - this.transform.position);
            distance = Vector3.Magnitude(planet.transform.position - this.transform.position);
            //velocity += (gravitationalConstant * planetScript.mass / Mathf.Pow(distance, 2)) * direction;
            rb.velocity += ((gravitationalConstant * planetRb.mass / Mathf.Pow(distance, 2)) * direction) * Time.deltaTime;
        }
    }

    // Collision with other objects
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Planet")
        {
            //Vector3 mov = Vector3.Reflect(velocity, collision.contacts[0].normal);
            //velocity = mov;
            rb.velocity = Vector3.Reflect(rb.velocity, collision.contacts[0].normal);
            
        }
    }

    public float GetGravitationalConstant()
    {
        return gravitationalConstant;
    }
}
