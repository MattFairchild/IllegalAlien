using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class TestSwitch : MonoBehaviour {

    public GameObject main;
    public GameObject options;

    public GameObject button;

    private float time = 5.0f;

	// Use this for initialization
	void Start () {
        options.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
        time -= Time.deltaTime;

        if (time < 0.0f)
        {
            main.SetActive(false);
            options.SetActive(true);
            EventSystem.current.firstSelectedGameObject = button;
        }
	}
}
