using UnityEngine;
using System.Collections;

public class MouseTest : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

        // print(Input.mousePosition);
        if(Input.GetMouseButtonDown(0))
        {
            //
            print("down");
        }
        else if(Input.GetMouseButton(0))
        {
            print("click");
        }
        else if(Input.GetMouseButtonUp(0))
        {
            print("up");
        }
        print("x="+Input.GetAxis("Mouse X")+"  y="+Input.GetAxis("Mouse Y"));
	}
}

