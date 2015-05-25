using UnityEngine;
using System.Collections;

public class GravityTexture : MonoBehaviour 
{
    public GameManager gm;

    private Texture2D mask;
    private int textureSize;
    private float grav;
    private float sphereSize;
    private float distance;
    private float speed;   

	// Use this for initialization
	void Start () 
    {
        gm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();

        float mass = GetComponentInParent<Rigidbody>().mass;
        textureSize = 512;
        //grav = GetComponent<TurretScript>().gravitationalConstant; //gameManager
        grav = 30f;

        sphereSize = 2 *  mass * grav / Mathf.Pow(1.75f, 2);
        transform.localScale = new Vector3(sphereSize, 0, sphereSize);

        mask = new Texture2D(textureSize, textureSize, TextureFormat.RGBA32, true);

        for (int j = textureSize; j >= textureSize / 2; j--) 
        {

            distance = 0.5f * sphereSize * ((((float)j / (float)textureSize) - 1) * -2);
            speed = Mathf.Sqrt(mass * grav / distance);
                
            Color tex = new Color(0,0,0);

            if (speed < 2f)
            {
                tex = new Color(speed / 2f, 0, 0, 0.25f * (4 * speed - 7f));
            }
            if (speed >= 2f && speed < 4f)
            {
                tex = new Color(1 - (speed - 2f) / 2f, (speed - 2f) / 2f, 0, 0.25f);
            }
            if (speed >= 4f && speed < 6f)
            {
                tex = new Color(0, (speed - 6f) / -2, 1 - (speed - 6f) / -2, 0.25f);
            }
            if (speed >= 6f)
            {
                tex = new Color(0, 0, (speed - 8f) / -2, 0.25f);
            }

            for (int i = 1; i <= textureSize; i++)
            {
                mask.SetPixel(i, j, tex); 
            }

        }
        mask.Apply();
        gameObject.GetComponent<Renderer>().material.mainTexture = mask;
	}


    void Update()
    {
        if (gm.showRadius)
        {
            gameObject.GetComponent<Renderer>().enabled = true;
        }
        else
        {
            gameObject.GetComponent<Renderer>().enabled = false;
        }
    }

}
