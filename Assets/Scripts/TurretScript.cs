using UnityEngine;
using System.Collections;

public class TurretScript : MonoBehaviour
{
    public Rigidbody rb;
    public Vector3 direction;
    public float distance;
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
        foreach (GameObject planet in planets)
        {
            Rigidbody planetRb = planet.GetComponent<Rigidbody>();
            direction = Vector3.Normalize(planet.transform.position - this.transform.position);
            distance = Vector3.Magnitude(planet.transform.position - this.transform.position);
            rb.velocity += ((gravitationalConstant * planetRb.mass / Mathf.Pow(distance, 2)) * direction) * Time.deltaTime;
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

    public float GetGravitationalConstant()
    {
        return gravitationalConstant;
    }
}
