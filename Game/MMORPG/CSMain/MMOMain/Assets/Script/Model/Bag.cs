using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public partial class Bag
{
    public delegate void BagEvent(List<Item> item);
    public event BagEvent OnBagAddItem;
    public event BagEvent OnBagDelItem;

    /// <summary>
    /// 客户端增加背包物品 不需要检查是否成功 由Handler调用
    /// </summary>
    /// <param name="itemList"></param>
    public void AddItem(List<Item> itemList)
    {
        foreach (Item item in itemList)
        {
            AddItem(item);
        }
        if (OnBagAddItem != null)
        {
            OnBagAddItem(itemList);
        }
    }

    /// <summary>
    /// 客户端的删除背包物品，不需要检查是否成功 由Handler调用
    /// </summary>
    /// <param name="itemList"></param>
    /// <returns></returns>
    public void DelItem(List<Item> itemList)
    {
        foreach(Item item in itemList)
        {
            DelItem(item);
        }
        if (OnBagAddItem != null)
        {
            OnBagDelItem(itemList);
        }
    }
}

