using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fpsCounter : MonoBehaviour {
    Rect fpsRect;
    GUIStyle style;

	// Use this for initialization
	void Start () {
        fpsRect = new Rect(100, 100, 400, 100);
        style = new GUIStyle();
        style.fontSize = 30;
	}
	
	// Update is called once per frame
	void Update () {
		        
	}
    private void OnGUI()
    {
        float fps = 1 / Time.deltaTime;
        GUI.Label(fpsRect, "FPS: " + fps, style);
    }
}
