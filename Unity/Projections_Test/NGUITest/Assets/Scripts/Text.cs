using UnityEngine;
using System.Collections;

public class Text : MonoBehaviour {
    private UITextList textList;
	// Use this for initialization
	void Start () {
        textList=this.GetComponent<UITextList>();
	}
	
	// Update is called once per frame
	void Update () {
        if(Input.GetMouseButtonDown(0))
        {
            textList.Add("aaa");
        }
        if(Input.GetMouseButtonDown(1))
        {
            textList.Add("bbb");
        }
	}
}
