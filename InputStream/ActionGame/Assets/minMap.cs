using UnityEngine;
using System.Collections;

//小地图脚本
public class minMap : MonoBehaviour {
    private Transform playerIcon;
    public static minMap map;
    public GameObject enemyIconPrefab;

	// Use this for initialization
	void Awake () {
        map = this;
        playerIcon = transform.Find("playerIcon");

	}
	
    public Transform getPlayerIcon()
    {
        return playerIcon;
    }
    public GameObject GetBossIcon()
    {
        GameObject go=NGUITools.AddChild(this.gameObject, enemyIconPrefab);
        go.GetComponent<UISprite>().spriteName = "BossIcon";
        return go;

    }
    public GameObject GetMonstIcon()
    {
        GameObject go = NGUITools.AddChild(this.gameObject, enemyIconPrefab);
        go.GetComponent<UISprite>().spriteName = "EnemyIcon";
        return go;
    }
}
