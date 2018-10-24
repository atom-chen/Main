using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour 
{
    public static PlayData PlayerData;

    public static NetManager NetManager;

    public static CallBackManager CallBackManager;

    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        NetManager = Utils.TryAddComponent<NetManager>(gameObject);
        CallBackManager = Utils.TryAddComponent<CallBackManager>(gameObject);
    }

    void Start()
    {
        SceneMgr.LoadScene(SCENE_CODE.LAUNCH);
    }
}
