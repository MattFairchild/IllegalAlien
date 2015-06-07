using UnityEngine;
using System.Collections;

public class ResourcesScript : MonoBehaviour {

    public int resources;

	void Start () {
        transform.localScale *= 0.25f * (float)resources;
	}

    void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            GameManager.addResouces(resources);
            Destroy(this.gameObject);
        }
    }
}
