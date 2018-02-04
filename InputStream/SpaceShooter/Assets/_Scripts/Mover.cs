using UnityEngine;
using System.Collections;

public class Mover : MonoBehaviour {
    public float speed = 10;
	void Start () {
         //从生成的位置开始移动
         this.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, speed);
	}
	
}
