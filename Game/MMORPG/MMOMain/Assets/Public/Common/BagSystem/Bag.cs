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


    private bool AddItem(Item item)
    {
        Item bagItem = GetItem(item.tabID);
        if (bagItem != null)
        {
            bagItem.count += item.count;
        }
        else
        {
            m_Bag.Add(item);
        }
        return true;
    }

    private bool DelItem(Item item)
    {
        Item bagItem = GetItem(item.tabID);
        if (bagItem != null)
        {
            if(bagItem.count>=item.count)
            {
                bagItem.count -= item.count;
                if(bagItem.count<=0)
                {
                    m_Bag.Remove(bagItem);
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

    public int Size
    {
        get
        {
            return m_Bag.Count;
        }
    }

    public Item GetItemByIndex(int index)
    {
        if(index<m_Bag.Count)
        {
            return m_Bag[index];
        }
        return null; 
    }

    public List<Item> GetList()
    {
        return m_Bag;
    }
    
}

