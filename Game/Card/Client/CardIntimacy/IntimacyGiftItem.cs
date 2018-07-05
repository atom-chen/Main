using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//********************************************************************
// 描述: 礼物背包-单个礼物的脚本
// 作者: wangbiyu
// 创建时间: 2018-3-8
//
//
//********************************************************************

public class IntimacyGiftItem : MonoBehaviour {
    public UISprite m_Icon;
    public UILabel m_Count;
    public Transform m_PitOn;

    private int m_DataId;
    public int DataID
    {
        get
        {
            return m_DataId;
        }
    }

    private ulong m_ItemGuid;
    public ulong Guid
    {
        get
        {
            return m_ItemGuid;
        }
    }

    private string m_ItemName = "";
    public string Name
    {
        get
        {
            return m_ItemName;
        }
    }

    private int m_ItemCount;
    public int Count
    {
        get
        {
            return m_ItemCount;
        }
        set
        {
            m_ItemCount = value;
        }
    }

    private string m_DescInTable = "";
    public string Desc
    {
        get
        {
            return m_DescInTable;
        }
    }

    private bool m_IsPit = false;
    //是否正在被选中
    public bool IsPitch
    {
        get
        {
           return m_IsPit;
        }
    }

    void OnEnable()
    {
        if(m_Icon==null)
        {
            m_Icon = this.GetComponentInChildren<UISprite>();
        }
        if(m_Count==null)
        {
            m_Count=this.GetComponentInChildren<UILabel>();
        }
        if(m_PitOn==null)
        {
            m_PitOn=this.transform.GetChild(4);
        }
    }
    //复位
    void OnDisable()
    {
        m_IsPit = false;
        m_PitOn.gameObject.SetActive(false);
    }
    public void InitGiftItem(string spriteName, int itemCount,string itemName,int dataID,ulong guid,string descInItemTable)
    {
        if(m_Icon!=null)
        {
            m_Icon.spriteName = spriteName;
        }
        if(m_Count!=null)
        {
            m_Count.text = itemCount+"";
        }
        m_ItemName = itemName;
        m_DataId = dataID;
        m_ItemGuid = guid;
        m_DescInTable = descInItemTable;
        m_ItemCount = itemCount;
    }

    //点击事件
    public void OnClickGift()
    {
        if(IntimacyGiftBag.Instance!=null)
        {
            IntimacyGiftBag.Instance.OnClickItem(this.DataID);
        }
    }

    //勾选/反勾选
    public void Pitch()
    {
        m_IsPit=!m_IsPit;
        m_PitOn.gameObject.SetActive(m_IsPit);
    }

    //打开或关闭选中框(根据布尔值)
    public void PitOn(bool isOpen)
    {
        m_IsPit = isOpen;
        m_PitOn.gameObject.SetActive(isOpen);
    }

}
