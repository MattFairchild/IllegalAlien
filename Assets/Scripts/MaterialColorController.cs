using UnityEngine;
using System.Collections;

public abstract class MaterialColorController : MonoBehaviour
{
    public bool Animate = true;

    public Gradient Colors;

    public float Frequence = 1;

    public string MaterialParameterName;

    protected float sinTime;

    void Update()
    {

    }
}
