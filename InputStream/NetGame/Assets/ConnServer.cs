using UnityEngine;
using System.Collections;

public class ConnServer : MonoBehaviour {
    public void Onclick()
    {
        Debug.Log("申请连接服务器");
        NewWorkScripts.getInstance();
    }
}
