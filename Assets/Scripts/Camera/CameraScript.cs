using System;
using UnityEngine;
using System.Collections;

public class CameraScript : MonoBehaviour
{
    public bool AnimateCamera;

    public float xBounds = 50.0f;
    public float zBounds = 20.0f;
    public float yDistance = 20.0f;

    public Vector3 startPosition;
    public Vector3 startRotation;

    private Transform player;
    private Camera cam;

    private const float camTransitionDuration = 4;
    private const float camStartTransitionTime = 0;


    void OnDrawGizmos()
    {
        Color regColor = Gizmos.color;
        Gizmos.color = Color.blue;

        //Gizmos.DrawLine();
        Gizmos.DrawWireCube(Vector3.zero + Vector3.up * yDistance, 2 * new Vector3(xBounds, 0, zBounds));

        Gizmos.color = regColor;
    }

    // Use this for initialization
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        this.transform.position = new Vector3(player.position.x, yDistance, player.position.z - 2);
        cam = GetComponent<Camera>();

    }

    // Update is called once per frame
    void Update()
    {
        float correctX, correctZ;
        const float correctY = 20.0f;
        bool correct = false;
        this.transform.position = new Vector3(player.position.x, this.transform.position.y, player.position.z - 2);

        if (transform.position.x < -xBounds)
        {
            correctX = -xBounds;
            correct = true;
        }
        else if (transform.position.x > xBounds)
        {
            correctX = xBounds;
            correct = true;
        }
        else
        {
            correctX = transform.position.x;
        }


        if (transform.position.z < -zBounds)
        {
            correctZ = -zBounds;
            correct = true;
        }
        else if (transform.position.z > zBounds)
        {
            correctZ = zBounds;
            correct = true;
        }
        else
        {
            correctZ = transform.position.z;
        }

        if (correct)
        {
            this.transform.position = new Vector3(correctX, correctY, correctZ);
        }

        float timeSinceStart = GameManager.TimeSinceStart;

        if (AnimateCamera && camStartTransitionTime <= timeSinceStart && timeSinceStart < camStartTransitionTime + camTransitionDuration)
        {
            float lerpFactor = Mathf.Clamp01((timeSinceStart - camStartTransitionTime) / camTransitionDuration);
            this.transform.position = Vector3.Slerp(startPosition, new Vector3(correctX, correctY, correctZ), lerpFactor);
            this.transform.rotation = Quaternion.Slerp(Quaternion.Euler(startRotation), Quaternion.Euler(90, 0, 0), lerpFactor);
        }



    }
}
