using UnityEngine;
using System.Collections;


/*
 
 
 
 This script is only needed for testing purposes. It allows to spawn a turret and give it an initial velocity 
 with the mouse at arbitrary positions. 
 
 
 
 */
public class SpawnTurret : MonoBehaviour
{

    public GameObject turretPrefab;
    public GameObject tempTurret;

    private Plane plane;
    private bool placed = false;
    private Ray ray;
    private LineRenderer lR; //will indicate initial speed and direction to shoot
    public Vector3 velocity; //speed to intantiate turret
    public Vector3 mousePos, spawnedPosition;
    private float hit;

    // Use this for initialization
    void Start()
    {
        plane = new Plane(Vector3.up, new Vector3(0.0f, 0.0f, 0.0f)); // X-Z plane

        //init linerenderer
        lR = this.GetComponent<LineRenderer>();
        lR.SetWidth(0.3f, 0.1f);
        lR.GetComponent<Renderer>().enabled = false;
    }

    // Update is called once per frame
    void Update()
    {

        //always keep track of where the mouse is on the screen
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (plane.Raycast(ray, out hit))
        {
            mousePos = ray.GetPoint(hit);
            velocity = mousePos - spawnedPosition;
        }

        if (!placed)
        {
            if (Input.GetMouseButtonDown(1))
            {
                tempTurret = (GameObject)GameObject.Instantiate(turretPrefab, mousePos, Quaternion.identity);

                spawnedPosition = mousePos;
                lR.GetComponent<Renderer>().enabled = true;
                placed = !placed;
            }
        }
        else
        {
            if (Input.GetMouseButtonDown(1))
            {
                tempTurret.GetComponent<TurretScript>().SetVelocity(velocity);

                lR.GetComponent<Renderer>().enabled = false;
                placed = !placed;
            }
        }


        UpdateLine();

    }

    void UpdateLine()
    {
        if (tempTurret)
        {
            lR.SetVertexCount(2);
            lR.SetPosition(0, tempTurret.transform.position);
            lR.SetPosition(1, tempTurret.transform.position + velocity);
        }
    }

}
