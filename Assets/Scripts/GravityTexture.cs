using UnityEngine;
using System.Collections;

public class GravityTexture : MonoBehaviour
{
    private Texture2D mask;
    private int textureSize;
    private float boxSize;
    private float distance;
    private float speed;
    

    // Use this for initialization
    void Start()
    {
        float mass = GetComponentInParent<Rigidbody>().mass;
        textureSize = 512;

        boxSize = 2 * mass * GameManager.getGravitationalConstant() / Mathf.Pow(1.5f, 2);
        transform.localScale = new Vector3(boxSize, 0, boxSize);

        mask = new Texture2D(textureSize, textureSize, TextureFormat.RGBA32, true);

        for (int j = 0; j < textureSize; j++)
        {


            for (int i = 0; i < textureSize; i++)
            {

                distance = 2f * boxSize * Mathf.Sqrt(Mathf.Pow(((float)i - (float)textureSize / 2f) / (float)textureSize / 2f, 2f) + Mathf.Pow(((float)j - (float)textureSize / 2f) / (float)textureSize / 2f, 2f));
                speed = Mathf.Sqrt(mass * GameManager.getGravitationalConstant() / distance);

                Color tex = new Color(0, 0, 0, 0);
                if (speed < 2f)
                {
                    tex = new Color((2 * speed - 3f), 0, 0, 0.25f * (2 * speed - 3f));
                }
                else if (speed >= 2f && speed < 3f)
                {
                    tex = new Color(1, speed - 2f, 0, 0.25f);
                }
                else if (speed >= 3f && speed < 4f)
                {
                    tex = new Color(4f - speed, 1, 0, 0.25f);
                }
                else if (speed >= 4f && speed < 6f)
                {
                    tex = new Color(0, (speed - 6f) / -2, 1 - (speed - 6f) / -2, 0.25f);
                }
                else if (speed >= 6f)
                {
                    tex = new Color(0, 0, (speed - 8f) / -2, 0.25f);
                }

                mask.SetPixel(i, j, tex);
            }

        }
        mask.Apply();
        gameObject.GetComponent<Renderer>().material.mainTexture = mask;
    }


    void Update()
    {
        if (GameManager.getShowRadius())
        {
            gameObject.GetComponent<Renderer>().enabled = true;
        }
        else
        {
            gameObject.GetComponent<Renderer>().enabled = false;
        }
    }


}

//Old version with spheres

//using UnityEngine;
//using System.Collections;

//public class GravityTexture : MonoBehaviour
//{
//    private Texture2D mask;
//    private int textureSize;
//    private float sphereSize;
//    private float distance;
//    private float speed;

//    // Use this for initialization
//    void Start()
//    {
//        float mass = GetComponentInParent<Rigidbody>().mass;
//        textureSize = 512;

//        sphereSize = 2 * mass * GameManager.getGravitationalConstant() / Mathf.Pow(1.75f, 2);
//        transform.localScale = new Vector3(sphereSize, 0, sphereSize);

//        mask = new Texture2D(textureSize, textureSize, TextureFormat.RGBA32, true);

//        for (int j = textureSize; j >= textureSize / 2; j--)
//        {

//            distance = sphereSize * ((((float)j / (float)textureSize) - 1) * -1);
//            speed = Mathf.Sqrt(mass * GameManager.getGravitationalConstant() / distance);

//            Color tex = new Color(0, 0, 0);

//            if (speed < 2f)
//            {
//                tex = new Color((4 * speed - 7f), 0, 0, 0.25f * (4 * speed - 7f));
//            }
//            else if (speed >= 2f && speed < 3f)
//            {
//                tex = new Color(1, speed - 2f, 0, 0.25f);
//            }
//            else if (speed >= 3f && speed < 4f)
//            {
//                tex = new Color(4f - speed, 1, 0, 0.25f);
//            }
//            else if (speed >= 4f && speed < 6f)
//            {
//                tex = new Color(0, (speed - 6f) / -2, 1 - (speed - 6f) / -2, 0.25f);
//            }
//            else if (speed >= 6f)
//            {
//                tex = new Color(0, 0, (speed - 8f) / -2, 0.25f);
//            }

//            for (int i = 1; i <= textureSize; i++)
//            {
//                mask.SetPixel(i, j, tex);
//            }

//        }
//        mask.Apply();
//        gameObject.GetComponent<Renderer>().material.mainTexture = mask;
//    }


//    void Update()
//    {
//        if (GameManager.getShowRadius())
//        {
//            gameObject.GetComponent<Renderer>().enabled = true;
//        }
//        else
//        {
//            gameObject.GetComponent<Renderer>().enabled = false;
//        }
//    }


//}

//                Color tex = new Color(0, 0, 0, 0);
//                if (speed < 2f)
//                {
//                    tex = new Color((2 * speed - 3f), 0, 0, 0.25f * (2 * speed - 3f));
//                }
//                else if (speed >= 2f && speed < 3f)
//                {
//                    tex = new Color(1, speed - 2f, 0, 0.25f);
//                }
//                else if (speed >= 3f && speed < 4f)
//                {
//                    tex = new Color(4f - speed, 1, 0, 0.25f);
//                }
//                else if (speed >= 4f && speed < 6f)
//                {
//                    tex = new Color(0, (speed - 6f) / -2, 1 - (speed - 6f) / -2, 0.25f);
//                }
//                else if (speed >= 6f)
//                {
//                    tex = new Color(0, 0, (speed - 8f) / -2, 0.25f);
//                }








//public Texture2D colors;
//private int colorsX;
//colorsX = (int)((speed - 1.5f) / 6f * (float)colors.width);
//tex = colors.GetPixel(colorsX, 1);



