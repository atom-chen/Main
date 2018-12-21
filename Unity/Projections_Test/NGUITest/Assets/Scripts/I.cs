using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class I : MonoBehaviour 
{
    int idx = 0;
	void Start () 
    {
        InvokeRepeating("Invvvv",0.5f,1.0f);
	}
	

    void Update()
    {
        if(idx>=5)
        {
            DestroyImmediate(this.gameObject);
        }
    }

    void Invvvv()
    {
        idx++;
    }
}
