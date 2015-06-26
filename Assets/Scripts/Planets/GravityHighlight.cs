using UnityEngine;
using System.Collections;

public class GravityHighlight : MonoBehaviour
{

    private float oldSpeed;
    private float currentSpeed;

    private const float lerpFactor = 0.25f;
    private const float speedNormalizationFactor = 10;

    void Update()
    {

        currentSpeed = GameManager.player.speed;
        currentSpeed = (1 - lerpFactor) * oldSpeed + lerpFactor * currentSpeed;

        //float scale = gameObject.GetComponentInParent<Transform>().lossyScale.x;
        float gravityOffset = Mathf.Sqrt(currentSpeed) * speedNormalizationFactor;

        gameObject.GetComponent<Renderer>().material.SetFloat("_Offset", gravityOffset);
        //gameObject.GetComponent<Renderer>().material.SetFloat("_Scale", scale);

        gameObject.GetComponent<Renderer>().enabled = GameManager.showRadius;

        oldSpeed = currentSpeed;
    }
}
