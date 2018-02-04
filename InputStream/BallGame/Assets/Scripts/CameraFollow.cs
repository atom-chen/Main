using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour {
    //拿到足球游戏对象
    public Transform FootBall;
    //主摄像机游戏对象
    public GameObject mainCamera;
    //高度阻尼
    public float heightDamping=0;
    //Z轴阻尼
    public float ZDamping=5;

	
	// Update is called once per frame
	void LateUpdate () {
        float currentHeight = mainCamera.transform.position.y;
        float wangtedHeight = 3;

        float currentRotationAngles = mainCamera.transform.eulerAngles.y;
        currentHeight = Mathf.Lerp(currentHeight, wangtedHeight, heightDamping * Time.deltaTime);
        Quaternion currentRotation = Quaternion.Euler(0, currentRotationAngles, 0);
        //平移摄像机
        mainCamera.transform.position = new Vector3(mainCamera.transform.position.x, currentHeight, mainCamera.transform.position.z);
        //如果在范围内 则始终注视
        if(mainCamera.transform.position.z<15.5)
        {
            mainCamera.transform.LookAt(FootBall.transform);
        }

	}
}
