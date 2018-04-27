using UnityEngine;
using System.Collections;
//tooltip:提示
public class tooltip : MonoBehaviour {
    void OnGUI()
    {
        GUI.Button(new Rect(Screen.width / 10, Screen.height / 10, Screen.width / 5, Screen.height / 10),
            new GUIContent(("点我"), "这是一个tooltip"));
        GUI.Label(new Rect(Screen.width / 10, Screen.height / 5, Screen.width / 5, Screen.height / 10),
            GUI.tooltip);
    }



	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
