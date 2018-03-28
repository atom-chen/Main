using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WaringManager : MonoBehaviour {
    public static List<WaringModel> errors=new List<WaringModel>();
    [SerializeField]
    private WaringWindow window;
    public static void addWaring(WaringModel text)
    {
        errors.Add(text);
    }
    private WaringManager()
    {

    }
	
	// Update is called once per frame
	void Update () {
        //每一帧检查并处理一个警告窗口
        if(errors.Count>0)
        {
            //拿到当前警告信息
           WaringModel err = errors[0];
            //移除警告窗口
            errors.RemoveAt(0);
            //激活警告信息
            window.active(err);
        }
	}

}
