using UnityEngine;
using System.Collections;

public class DestoryByTime : MonoBehaviour {

    public float liftTime = 2;
	void Start () {
        Destroy(this.gameObject, liftTime);
	}
	

}
