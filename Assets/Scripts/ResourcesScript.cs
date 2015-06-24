using UnityEngine;
using System.Collections;

public class ResourcesScript : MonoBehaviour {

    protected int m_resources;

    void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            GameManager.addResouces(resources);
            Destroy(this.gameObject);
        }
    }

	public int resources {
		get { return m_resources; }
		set { m_resources = value; transform.localScale = Vector3.one * 0.66f * Mathf.Sqrt((float)m_resources); }
	}
}
