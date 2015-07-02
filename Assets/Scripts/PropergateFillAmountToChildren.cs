using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PropergateFillAmountToChildren : MonoBehaviour
{

    private Image[] Images;

    void Start()
    {
        Images = GetComponentsInChildren<Image>();
    }

    void Update()
    {
        for (int i = 1; i < Images.Length; i++)
        {
            Images[i].fillAmount = GetComponent<Image>().fillAmount;
        }
    }
}
