using UnityEngine;
using System.Collections;
using UnityEditor;

public class GravityTexture : MonoBehaviour
{
    private Texture2D mask;
    private int textureSize;
    private float boxSize;
    private float distance;
    private float speed;
    private float alpha;
    public Texture2D inputTexture;
    private int inputTextureX;
    

    // Use this for initialization
    void Start()
    {
        float mass = GetComponentInParent<Rigidbody>().mass;
        textureSize = 512;

        alpha = 0.20f;

        boxSize = 2 * mass * GameManager.getGravitationalConstant() / Mathf.Pow(1.5f, 2);
        transform.localScale = new Vector3(boxSize, 0, boxSize);
        GetComponentInParent<PlanetScript>().range = boxSize;
        mask = new Texture2D(textureSize, textureSize, TextureFormat.RGBA32, true);

        StartCoroutine(CreateTexture(mass));
        
    }

    void Update()
    {
        if (GameManager.showRadius)
        {
            gameObject.GetComponent<Renderer>().enabled = true;
        }
        else
        {
            gameObject.GetComponent<Renderer>().enabled = false;
        }
    }

    IEnumerator CreateTexture(float mass)
    {
        for (int j = 0; j < textureSize; j++)
        {
            for (int i = 0; i < textureSize; i++)
            {

                distance = 2f * boxSize * Mathf.Sqrt(Mathf.Pow(((float)i - (float)textureSize / 2f) / (float)textureSize / 2f, 2f) + Mathf.Pow(((float)j - (float)textureSize / 2f) / (float)textureSize / 2f, 2f));
                speed = Mathf.Sqrt(mass * GameManager.getGravitationalConstant() / distance);

                Color tex = new Color(0, 0, 0, 0);
                
                if (speed < 2f)
                {
                    if (speed >= 1.5f)
                    {
                        inputTextureX = (int)((speed / 10f) * (float)(inputTexture.width));
                        tex = inputTexture.GetPixel(inputTextureX, 1);
                        tex.a = (speed - 1.5f) * 2f * alpha;
                    }
                }
                else if (speed > 6f)
                {
                    if (speed <= 10f)
                    {
                        inputTextureX = (int)((speed / 10f) * (float)(inputTexture.width));
                        tex = inputTexture.GetPixel(inputTextureX, 1);
                        tex.a = (-speed + 10f) * 0.25f * alpha;
                    }
                }
                else
                {
                    inputTextureX = (int)((speed / 10f) * (float)(inputTexture.width));
                    tex = inputTexture.GetPixel(inputTextureX, 1);
                    tex.a = alpha;
                }
                
                
                mask.SetPixel(i, j, tex);
            }
         
        }
        mask.Apply();
        gameObject.GetComponent<Renderer>().material.mainTexture = mask;

        yield break;
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

//if (speed < 2f)
//                {
//                    tex = new Color((2 * speed - 3f), 0, 0, alpha * (2 * speed - 3f));
//                }
//                else if (speed >= 2f && speed < 3f)
//                {
//                    tex = new Color(1, speed - 2f, 0, alpha);
//                }
//                else if (speed >= 3f && speed < 4f)
//                {
//                    tex = new Color(4f - speed, 1, 0, alpha);
//                }
//                else if (speed >= 4f && speed < 6f)
//                {
//                    tex = new Color(0, (speed - 6f) / -2, 1 - (speed - 6f) / -2, alpha);
//                }
//                else if (speed >= 6f)
//                {
//                    tex = new Color(0, 0, (speed - 8f) / -2, alpha);
//                }      



//              public Texture2D inputTexture;
//              private int inputTextureX;
//              inputTextureX = (int)((speed - 1.5f) / 6f * (float)inputTexture.width);
//              tex = colors.GetPixel(inputTextureX, 1);


//if (speed < 2f)
//                {
//                    tex = EditorGUIUtility.HSVToRGB(0, 1f, 1f);
//                    tex.a = (speed - 1.5f) * 2f * alpha;
//                }
//                else if (speed > 6f)
//                {
//                    tex = EditorGUIUtility.HSVToRGB(0.9f, 1f, 1f);
//                    tex.a = (speed - 6f) * 0.5f * alpha;
//                }
//                else
//                {
//                    tex = EditorGUIUtility.HSVToRGB((speed - 2f) / 4.444f, 1f, 1f);
//                    tex.a = alpha;
//                }   



