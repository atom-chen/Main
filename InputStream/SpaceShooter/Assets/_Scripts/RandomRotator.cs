using UnityEngine;
using System.Collections;

//小行星的随机旋转
public class RandomRotator : MonoBehaviour {
    public float tumble = 10;
	// Use this for initialization
	void Start () {
        this.GetComponent<Rigidbody>().angularVelocity = Random.insideUnitSphere * tumble;
        
	}
	
}
