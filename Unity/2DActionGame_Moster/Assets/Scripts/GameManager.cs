using UnityEngine;
using System.Collections;
//player脚本，让Player一直往前走

public class GameManager : MonoBehaviour {
    public Player m_Player;

    public static GameManager _Instance;
    void Awake()
    {
        _Instance = this;
    }
    void Update()
    {
        if(m_Player.transform.position.x>=TerrainManager.Instance.m_MidPos)
        {
            //移动
            TerrainManager.Instance.FloorMove();
        }
    }

}
