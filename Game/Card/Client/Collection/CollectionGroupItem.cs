using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Games.Table;
using Games.GlobeDefine;
public class CollectionGroupItem : MonoBehaviour 
{
    public UILabel m_GroupName;
    public GameObject m_RedPoint;
    private int mId = GlobeVar.INVALID_ID;
    public int Id
    {
        get { return mId; }
    }
    public void Refresh(Tab_Handbook tHandBook)
    {
        if(tHandBook == null)
        {
            return;
        }
        mId = tHandBook.Id;
        m_GroupName.text = tHandBook.Group2Name;
        m_RedPoint.SetActive(CollectionRedTool.IsCanAward_Handbook(tHandBook));
    }

    public void OnClickItem()
    {
        if(CollectionFulingController.Instance!=null)
        {
            CollectionFulingController.Instance.HandleOnGroupItemClick(this.Id);
        }
    }
}
