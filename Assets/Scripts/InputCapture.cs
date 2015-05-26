using UnityEngine;
using System.Collections;

public class InputCapture : MonoBehaviour {

    private float speed;
    private float flightAngle;
    private float shootAngle;

    private float speedR;
    private float speedU;

    private bool turret;
    private bool shoot;

    private int numOfPads;
    private Camera camera;
    private Vector3 screenUp, screenRight;

    //variables for mouse rotation
    private Plane plane;
    private Ray ray;
    private float hit;
    public Vector3 mouseAngle;

	// Use this for initialization
	void Start () {
        camera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        screenUp = camera.transform.up;
        screenRight = camera.transform.right;
        plane = new Plane(Vector3.up, new Vector3(0.0f, 0.0f, 0.0f)); // X-Z plane
	}
	
	// Update is called once per frame
	void Update () {

        //update the angle to rotate
        //Gamepad
        if ((numOfPads = numberOfGamepads()) != 0)
        {
            //move position up/down + left/right on the screen
            speedR = Input.GetAxis("HorizontalLeft");
            speedU = Input.GetAxis("VerticalLeft");
            speed = new Vector3(speedR, 0.0f, speedU).magnitude;


            //rotate towards how the right analog stick is facing
            float y = Input.GetAxis("HorizontalRight");
            float x = Input.GetAxis("VerticalRight");

            flightAngle = (Mathf.Atan2(speedU,speedR) * Mathf.Rad2Deg) - 90.0f;
            shootAngle = (Mathf.Atan2(y, x) * Mathf.Rad2Deg) + 90.0f;


            if (Input.GetAxis("ShootGamepad") > 0.1f)
            {
                shoot = true;
            }
            else
            {
                shoot = false;
            }


            if (Input.GetAxis("TurretGamepad") > 0.1f)
            {
                turret = true;
            }
            else
            {
                turret = false;
            }
        }
        //Mouse&Keyboard
        else
        {
            //see where the mouse is and calculate angle to rotate
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (plane.Raycast(ray, out hit))
            {
                mouseAngle = ray.GetPoint(hit);
                mouseAngle = mouseAngle - this.transform.position;
            }

            //only in range [0, 180], so use cross prodcut to know direction
            shootAngle = Vector3.Angle(screenUp, mouseAngle);

            //use cross product to in the end have the [0, 360] degree range
            Vector3 cross = Vector3.Cross(screenUp, mouseAngle);
            if (cross.y > 0.0f)
            {
                shootAngle = 360 - shootAngle;
            }


            //get speed horizontally and vertically
            speedR = Input.GetAxis("Horizontal");
            speedU = Input.GetAxis("Vertical");
            speed = new Vector3(speedR, 0.0f, speedU).magnitude;

            Vector3 screenPos = camera.WorldToScreenPoint(this.transform.position);


            if (Input.GetAxis("ShootMouse") > 0.1f)
            {
                shoot = true;
            }
            else
            {
                shoot = false;
            }


            if (Input.GetAxis("TurretKeyboard") > 0.1f)
            {
                turret = true;
            }
            else
            {
                turret = false;
            }
        }

	}


    //go through the Joystick array and check if any entries are non-empty.
    //If so, return number of Joysticks
    public int numberOfGamepads()
    {
        int count = 0;
        for (int i = 0; i < Input.GetJoystickNames().Length; i++)
        {
            if (Input.GetJoystickNames()[i].CompareTo("") != 0)
            {
                count++;
                Debug.Log("Controller Found " + i);
            }
        }

        return count;
    }


    /*
        A bunch of getters
     */

    public float getSpeed()
    {
        return speed;
    }

    public float getFlightAngle()
    {
        return flightAngle;
    }

    public float getShootAngle()
    {
        return shootAngle;
    }

    public bool isShooting()
    {
        return shoot;
    }

    public bool placingTurret()
    {
        return turret;
    }

    public float getSpeedRight()
    {
        return speedR;
    }

    public float getSpeedUp() 
    {
        return speedU;
    }

    public bool rightAnalogMoving()
    {
        float y = Mathf.Abs(Input.GetAxis("HorizontalRight"));
        float x = Mathf.Abs(Input.GetAxis("VerticalRight"));

        if (y > 0.1f || x > 0.1f)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
