using UnityEngine;
using System.Collections;

public class GravityHighlight : MonoBehaviour
{

    private float oldSpeed;
    private float currentSpeed;

    private const float lerpFactor = 0.1f;
    private const float speedNormalizationFactor = 10;

    void Update()
    {

        currentSpeed = GameManager.player.speed;
        currentSpeed = (1 - lerpFactor) * oldSpeed + lerpFactor * currentSpeed;

        gameObject.GetComponent<Renderer>().material.SetFloat("_Offset", currentSpeed * speedNormalizationFactor);
        gameObject.GetComponent<Renderer>().material.SetFloat("_Scale", gameObject.GetComponent<Transform>().lossyScale.x);

        gameObject.GetComponent<Renderer>().enabled = GameManager.showRadius;

        oldSpeed = currentSpeed;
    }
}
