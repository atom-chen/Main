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

