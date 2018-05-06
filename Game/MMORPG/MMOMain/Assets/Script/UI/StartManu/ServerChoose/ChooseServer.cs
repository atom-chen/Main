using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChooseServer : MonoBehaviour {
  public ServerItem m_CurServer;
  public ServerList m_ServerList;

  public void SetServerList(List<ServerProperty> serverList)
  {
    m_ServerList.InitServerList(serverList);
    m_CurServer.InitItem(serverList[0]);
  }

}
