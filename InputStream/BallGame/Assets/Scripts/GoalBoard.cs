using UnityEngine;
using System.Collections;

public class GoalBoard : MonoBehaviour {
    //偏移坐标
    private Vector2 offset;
    private Material material;

	// Use this for initialization
	void Start () {
        offset.x = Random.Range(0, 0.5f);
        offset.y = Random.Range(0, 0.5F);
        material = this.GetComponent<Renderer>().material;
	}
	
	// Update is called once per frame
	void Update () {
        //使坐标发生变化
        offset.x = Mathf.PingPong(Time.deltaTime * 0.05f, 0.5f);
        offset.x = Mathf.PingPong(Time.deltaTime * 0.17f, 0.5f);
        //进行纹理偏移
        material.SetTextureOffset("_MainTex", offset);

        //储存靶子坐标
        Staticer.goalX = Staticer.goalStartX + Staticer.goalWidth * offset.x / 0.5f;
        Staticer.goalY = Staticer.goalStartY + Staticer.goalHeight * offset.y / 0.5f;
	}
}
