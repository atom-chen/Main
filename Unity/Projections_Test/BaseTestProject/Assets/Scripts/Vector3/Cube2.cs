using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cube2 : MonoBehaviour {

	void Update () {
    transform.Translate(Vector3.forward* Time.deltaTime, Space.World);//沿着直接坐标系0,0,1 每秒移动一个单位
	}
}
