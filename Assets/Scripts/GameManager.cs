using UnityEngine;
using System.Collections;

//Class that should hold any type of infprmation needed in multiple other different scripts
public class GameManager : MonoBehaviour {
    public bool showRadius;
    public bool alwayShowRadius;
    public bool onlyNearest;

    void Start()
    {
        showRadius = false;
        onlyNearest = false;
        alwayShowRadius = false;
    }
}
