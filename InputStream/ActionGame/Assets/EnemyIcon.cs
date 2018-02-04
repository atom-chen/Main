using UnityEngine;
using System.Collections;

public class EnemyIcon : MonoBehaviour {
    private Transform icon;
    private Transform playerTrans;
	// Use this for initialization
	void Start () {
        playerTrans = GameObject.FindGameObjectWithTag(Tags.player).transform;
        //创建图标
        if(this.tag==Tags.soulBoss)
        {
            //得到BOSS图标
            icon = minMap.map.GetBossIcon().transform;
        }else if(this.tag==Tags.soulMonst)
        {
            icon = minMap.map.GetMonstIcon().transform;
        }
	}
	
	// Update is called once per frame
	void Update () {
        //拿到位置差
        Vector3 offset = transform.position - playerTrans.position;
        offset *= 7;
        icon.localPosition = new Vector3(offset.x, offset.z, 0);
	}
    void OnDestroy()
    {
        if(icon!=null)
        {
            Destroy(icon.gameObject);
        }
    }
}








