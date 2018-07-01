using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChooseServer : MonoBehaviour {
  public ServerListLogic m_ServerList;
  public Transform m_CurServerTrans;
  void OnEnable()
  {
    GetServerList();
  }
  private void GetServerList()
  {
    if (PhotoEngine.Instance != null)
    {
      PhotoEngine.Instance.SendRequest(OperationCode.GetServer, null);
    }
  }
  public void SetServerList(List<ServerPropert> serverList, ServerPropert curServer)
  {
    for (int i = 0; i < m_CurServerTrans.childCount;i++)
    {
      Destroy(m_CurServerTrans.GetChild(i).gameObject);
    }
    
    m_ServerList.InitServerList(serverList);
    if(curServer!=null)
    {
      if (curServer.Hot)
      {
        ServerItem CurServerItem = NGUITools.AddChild(m_CurServerTrans.gameObject, m_ServerList.m_HotServerPrefab).GetComponent<ServerItem>();
        if (CurServerItem != null)
        {
          CurServerItem.InitItem(curServer);
        }
      }
      else
      {
        ServerItem CurServerItem = NGUITools.AddChild(m_CurServerTrans.gameObject, m_ServerList.m_NormalServerPrefab).GetComponent<ServerItem>();
        if (CurServerItem != null)
        {
          CurServerItem.InitItem(curServer);
        }
      }
    }

  }

}
