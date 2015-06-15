using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlanetScript : MonoBehaviour
{
    public Rigidbody rb;

    // Use this for initialization
    void Awake()
    {
		rb = this.GetComponent<Rigidbody> ();
        rb.mass = transform.localScale.x;
    }

	public void scale(float val)
	{
		transform.localScale = transform.localScale * val;
		rb.mass = transform.localScale.x;
	}

	public void scaleTo(float val)
	{
		transform.localScale = new Vector3(val, val, val);
		rb.mass = transform.localScale.x;
	}
	
	public float getMass()
	{
		return rb.mass;
	}
}
