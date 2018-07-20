using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BagList : MonoBehaviour 
{
    public UIWrapContent m_Wrap;


    void OnEnable()
    {
        InitWindow();
        m_Wrap.onInitializeItem += OnWrapSlide;

    }
    void OnDisable()
    {
        m_Wrap.onInitializeItem -= OnWrapSlide;
    }


    /// <summary>
    /// 从Player中取出装备背包和物品背包显示
    /// </summary>
    void InitWindow()
    {
        m_Wrap.enabled = false;
        int length = PlayData.RoleData.bag.Size + PlayData.RoleData.equipBag.Size;
        m_Wrap.minIndex = -length + 1;
        m_Wrap.maxIndex = 0;
        m_Wrap.enabled = true;
    }

    /// <summary>
    /// 滑动时被调用
    /// </summary>
    void OnWrapSlide(GameObject obj,int wrapIndex,int dataIndex)
    {

    }

    //取出背包+装备背包拼接起来的容器 index指定信息
    private void GetDataItem(int dataIndex,out ITEM_FIRST type,out int id)
    {
        if(dataIndex<0)
        {
            type = ITEM_FIRST.INVALID;
            id = Define._INVALID_ID;
            return;
        }
        //如果在物品背包的范围
        if(dataIndex<PlayData.RoleData.bag.Size)
        {

        }
        //如果在装备背包的范围
        else if(dataIndex<(PlayData.RoleData.bag.Size + PlayData.RoleData.equipBag.Size))
        {
            dataIndex -= PlayData.RoleData.bag.Size;

        }
        else
        {
            //越界
            type = ITEM_FIRST.INVALID;
            id = Define._INVALID_ID;
        }
    }
}
