using UnityEngine;
using System.Collections;

//功能：过1秒销毁对象
public class AutoDestroy : MonoBehaviour {
    public float exitTime = 1;
	// Use this for initialization
	void Start () {
        Destroy(this.gameObject, exitTime);
	}
}
