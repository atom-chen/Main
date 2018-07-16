using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public partial class Bag
{
    public delegate void ItemEvent(Item item,int count);
    public static event ItemEvent OnUserItemSuccess;

    /// <summary>
    /// 增加新物品
    /// </summary>
    /// <param name="item"></param>
    public bool AddItem(Item item)
    {
        Item bagItem = GetItem(item.tabID);
        //如果集合中已经有该道具
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

    /// <summary>
    /// 使用count个该物品
    /// </summary>
    /// <param name="tabId">物品tabid</param>
    /// <param name="count">要删除的数量</param>
    /// <returns>是否成功</returns>
    public bool UseItem(int tabId, int count = 1)
    {
        Item item;
        if((item=DeleteItem(tabId,count))!=null)
        {
            if(OnUserItemSuccess!=null)
            {
                OnUserItemSuccess(item, count);
            }
            return true;
        }
        else
        {
            return false;
        }
    }

    /// <summary>
    /// 直接删除count个该物品
    /// </summary>
    public Item DeleteItem(int tabID,int count=1)
    {
        Item item = GetItem(tabID);
        if (item != null)
        {
            if (item.count >= count)
            {
                item.count -= count;
                if (item.count == 0)
                {
                    m_Bag.Remove(item);
                }
                return item;           //返回减去数量后的物品
            }
            else
            {
                return null;          //物品数量不足，返回空
            }
        }
        else
        {
            return null;             //没有该物品，返回空
        }
    }
}

