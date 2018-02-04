using UnityEngine;
using System.Collections;

public class Biu : MonoBehaviour {
    //拿到球网游戏对象
    public Transform qiuWang;
    public float speed; 

	// Use this for initialization
	void Start () {
        qiuWang = GameObject.Find("Box01").transform;
        //计算速度
        speed=Vector3.Distance(qiuWang.position, this.transform.position)/500;
        print(speed);
	}
	
	// Update is called once per frame
	void Update () {
        //如果玩家点击屏幕
        if(Input.GetMouseButtonDown(0))
        {
            //射门，获取触点方向
            Vector3 targetPos =Vector3.Normalize(Input.mousePosition);

            
        }
	}
}
