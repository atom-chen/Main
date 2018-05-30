using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class earth : MonoBehaviour {
  float timer = 0;
  float cd = 0.1f;
  float angle = 0;
	// Update is called once per frame
	void Update () {
    timer += Time.deltaTime;
    if(timer>=cd)
    {
      timer = 0;
      RoteteSelf();
    }

	}
  void RoteteSelf()
  {
    transform.Rotate(0,15,0);
  }
}
