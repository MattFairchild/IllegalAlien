using UnityEngine;
using System.Collections;
using UnityEngine.UI;

[ExecuteInEditMode]
public class ImageColorChanger : MonoBehaviour
{
    public Gradient Color;
    private Image image;

    // Use this for initialization
    void Start()
    {
        image = this.GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        image.color = Color.Evaluate(1 - image.fillAmount);
    }
}
