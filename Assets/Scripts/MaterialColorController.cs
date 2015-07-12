using UnityEngine;
using System.Collections;

public class MaterialColorController : MonoBehaviour
{
    public bool Animate = true;

    public Gradient Colors;

    public float Frequence = 1;

    public string MaterialParameterName;

    public Material[] Materials;

    private float sinTime;

    void Update()
    {
        if (Animate)
        {
            sinTime = Mathf.Sin(Time.time * Frequence);
            sinTime = sinTime / 2f + 0.5f;
            sinTime = Mathf.Clamp01(sinTime);

            foreach (Material material in Materials)
            {
                material.SetColor(MaterialParameterName, Colors.Evaluate(sinTime));
            }
        }
    }
}
