using UnityEngine;
using System.Collections;

public class LocalMaterialColorController : MaterialColorController
{
    void Update()
    {
        if (Animate)
        {
            sinTime = Mathf.Sin(Time.time * Frequence);
            sinTime = sinTime / 2f + 0.5f;
            sinTime = Mathf.Clamp01(sinTime);

            GetComponent<MeshRenderer>().material.SetColor(MaterialParameterName, Colors.Evaluate(sinTime));

        }
    }
}
