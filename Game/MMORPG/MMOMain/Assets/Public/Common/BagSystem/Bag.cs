using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/// <summary>
/// 普通物品背包
/// </summary>
public partial class Bag
{
    private List<Item> m_Bag;
    
    public Bag()
    {
        m_Bag = new List<Item>();
    }
    public Bag(List<Item> itemList)
    {
        m_Bag = itemList;
    }

    /// <summary>
    /// 增加新物品
    /// </summary>
    /// <param name="item"></param>
    public bool AddItem(Item item)
    {
        Item bagItem=GetItem(item.tabID);
        //如果集合中已经有该道具
        if (bagItem!=null)
        {
            bagItem.count += item.count;
        }
        else
        {
            m_Bag.Add(item);
        }
        return true;
    }

    /// <summary>
    /// 在背包中删除count个物品
    /// </summary>
    /// <param name="tabId">物品tabid</param>
    /// <param name="count">要删除的数量</param>
    /// <returns>是否成功</returns>
    public bool DeleteItem(int tabId,int count=1)
    {
        Item item = GetItem(tabId);
        if(item!=null)
        {
            if (item.count >= count)
            {
                item.count -= count;
                if(item.count==0)
                {
                    m_Bag.Remove(item);
                }
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return false;
        }
    }

    /// <summary>
    /// 集合中是否有该id的物品
    /// </summary>
    /// <param name="tabId">物品tabID</param>
    /// <returns></returns>
    public Item GetItem(int tabId)
    {
        foreach(Item item in m_Bag)
        {
            if(item.tabID==tabId)
            {
                return item;
            }
        }
        return null;
    }

}

