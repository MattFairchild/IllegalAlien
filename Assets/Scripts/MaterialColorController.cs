using UnityEngine;
using System.Collections;

public class MaterialColorController : MonoBehaviour
{
    public bool Animate = true;

    public Color startColor;
    public Color endColor;

    public float Frequence = 1;

    public string MaterialParameterName;

    public Material[] Materials;



    private Color[] startColors;

    private float sinTime;

    void Update()
    {
        if (Animate)
        {
            sinTime = Mathf.Sin(Time.time * Frequence);
            sinTime = (sinTime + 1) / 2f;

            for (int i = 0; i < Materials.Length; i++)
            {
                Materials[i].SetColor(MaterialParameterName, Color.Lerp(startColor, endColor, sinTime));
            }
        }
    }
}
