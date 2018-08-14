using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServerListLogic : MonoBehaviour
{
    public UIGrid m_Grid;
    public GameObject m_HotServerPrefab;
    public GameObject m_NormalServerPrefab;
    public void InitServerList(List<Tab_Server> serverList)
    {
        List<Transform> childList = m_Grid.GetChildList();
        for (int i = 0; i < childList.Count; i++)
        {
            DestroyImmediate(childList[i].gameObject);
        }
        foreach (Tab_Server item in serverList)
        {
            ServerItem serverItem = null ;
            if (item.Hot)
            {
                serverItem = NGUITools.AddChild(m_Grid.gameObject, m_HotServerPrefab).GetComponent<ServerItem>();
            }
            else
            {
                serverItem = NGUITools.AddChild(m_Grid.gameObject, m_NormalServerPrefab).GetComponent<ServerItem>();
            }
            if (serverItem != null)
            {
                serverItem.InitItem(item);
            }

        }
        m_Grid.Reposition();
    }
}
