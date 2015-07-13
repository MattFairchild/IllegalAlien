using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEditor;

public class FrameActivator : MonoBehaviour
{
    public float Speed;

    private List<CanvasRenderer> allCanvasRenderer = new List<CanvasRenderer>();

    private int time;

    // Use this for initialization
    void Start()
    {
        CanvasRenderer[] allCanvasRendererArray = this.GetComponentsInChildren<CanvasRenderer>();

        foreach (CanvasRenderer canvasRenderer in allCanvasRendererArray)
        {
            canvasRenderer.SetAlpha(0);
            allCanvasRenderer.Add(canvasRenderer);
        }

        allCanvasRenderer[0].SetAlpha(1);
        allCanvasRenderer.RemoveAt(0);

    }

    // Update is called once per frame
    void Update()
    {
        time = (int)(Time.time * Speed);
        time = time % allCanvasRenderer.Count;

        allCanvasRenderer[time - 1 < 0 ? allCanvasRenderer.Count - 1 : time - 1].SetAlpha(0);

        allCanvasRenderer[time].SetAlpha(1);
    }
}
