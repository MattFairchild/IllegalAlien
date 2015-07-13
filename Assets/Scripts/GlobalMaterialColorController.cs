using UnityEngine;

public class GlobalMaterialColorController : MaterialColorController
{

    public Material[] Materials;

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