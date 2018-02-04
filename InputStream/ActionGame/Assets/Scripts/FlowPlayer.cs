using UnityEngine;
using System.Collections;

public class FlowPlayer : MonoBehaviour {
    private Transform player;

    public float speed = 6f;

	// Use this for initialization
	void Start () {
        //拿到player标注的游戏对象 的transform组件引用
        player = GameObject.FindGameObjectWithTag(Tags.player).transform;
	}
	
	// Update is called once per frame
	void Update () {
        //位置跟随：新位置=玩家当前位置+固定的位置差
        Vector3 targerPos = player.position + new Vector3(0, 3f, -3f);
        //移动到新位置，利用public static Vector3 Lerp（Vector3 from， Vector3 to，float t）;
        transform.position = Vector3.Lerp(transform.position, targerPos, speed*Time.deltaTime);
        //修改摄像机朝向
        Quaternion targetRotation = Quaternion.LookRotation(player.position - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, speed * Time.deltaTime);


	}
}

