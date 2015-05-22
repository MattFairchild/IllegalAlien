using UnityEngine;
using System.Collections;

public class BulletScript : MonoBehaviour {

    public Rigidbody rb;
    public float speed = 25.0f;
    public float lifetime = 10.0f;

	// Use this for initialization
	void Start () {
        this.transform.rotation = Quaternion.AngleAxis(90.0f, this.transform.right);
        rb.velocity = this.transform.up * speed;
	}

    public void Update()
    {
        lifetime -= Time.deltaTime;

        if (lifetime < 0.0f)
        {
            GameObject.Destroy(this.gameObject);
        }
    }

    public void SetVelocity(Vector3 vel)
    {
        rb.velocity = vel;
    }

    private void OnCollisionEnter(Collision collision)
    { 
        GameObject.Destroy(this.gameObject);
    }
}
