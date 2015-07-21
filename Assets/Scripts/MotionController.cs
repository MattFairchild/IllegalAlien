using UnityEngine;
using System.Collections;
using System.Runtime.CompilerServices;

public class MotionController : MonoBehaviour
{

    public Vector3 TranslationOverTime;
    public Vector3 RotationOverTime;
    public Vector3 ScaleOverTime;

    public float TranslationSpeed = 1;
    public float RotationSpeed = 1;
    public float ScaleSpeed = 1;

    public bool UseSinusTime = false;
    public float SinusTimeOffset = 0;

    public float TranslationSinusOffset = 0;
    public float RotationSinusOffset = 0;
    public float ScaleSpeedSinusOffset = 0;


    private float linearTime;
    private float sinusTranslationTime;
    private float sinusRotationTime;
    private float sinusScaleTime;

    private Vector3 startTranslation;
    private Quaternion startRotation;
    private Vector3 startScale;

    private void Start()
    {
        startTranslation = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        startRotation = new Quaternion(transform.rotation.x, transform.rotation.y, transform.rotation.z, transform.rotation.w);
        startScale = new Vector3(transform.localScale.x, transform.localScale.y, transform.localScale.z);
    }

    // Update is called once per frame
    void Update()
    {
        linearTime = 2 * Time.deltaTime;

        transform.position = Vector3.Lerp(transform.position, transform.position + TranslationOverTime * TranslationSpeed * linearTime, 0.5f);
        transform.rotation = Quaternion.Lerp(transform.rotation, transform.rotation * Quaternion.Euler(RotationOverTime * RotationSpeed * linearTime), 0.5f);
        transform.localScale = Vector3.Lerp(transform.localScale, transform.localScale + ScaleOverTime * ScaleSpeed * linearTime, 0.5f);

        if (UseSinusTime)
        {
            sinusTranslationTime = 2 * Mathf.Sin((Time.time * TranslationSpeed + SinusTimeOffset) + (TranslationSinusOffset * Mathf.PI));
            sinusRotationTime = 2 * Mathf.Sin((Time.time * RotationSpeed + SinusTimeOffset) + (RotationSinusOffset * Mathf.PI));
            sinusScaleTime = 2 * Mathf.Sin((Time.time * ScaleSpeed + SinusTimeOffset) + (ScaleSpeedSinusOffset * Mathf.PI));

            transform.position = Vector3.Lerp(startTranslation, startTranslation + TranslationOverTime * sinusTranslationTime, 0.5f);
            transform.rotation = Quaternion.Lerp(startRotation, startRotation * Quaternion.Euler(RotationOverTime * sinusRotationTime), 0.5f);
            transform.localScale = Vector3.Lerp(startScale, startScale + ScaleOverTime * sinusScaleTime, 0.5f);

        }
    }
}
