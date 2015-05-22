using UnityEngine;
using System.Collections;

public class BulletScript : MonoBehaviour {

    public Rigidbody rb;
    public float speed = 25.0f;

	// Use this for initialization
	void Start () {
        this.transform.rotation = Quaternion.AngleAxis(90.0f, this.transform.right);
        rb.velocity = this.transform.up * speed;
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
