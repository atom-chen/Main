﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DB;

public class BagManager
{
    /// <summary>
    /// 获取属于role的所有物品，返回
    /// </summary>
    /// <param name="roleID">角色ID</param>
    /// <returns>物品list</returns>
    public static List<Item> GetItemFromRole(int roleID)
    {
        try
        {
            using (var session = NHibernateHelper.OpenSession())
            {
                using (var transction = session.BeginTransaction())
                {
                    var servers = session.QueryOver<_DBItem>().Where(item => item.roleID == roleID);
                    transction.Commit();
                    List<Item> ret = BuildListItem(servers.List());
                    return ret;
                }
            }
        }
        catch (Exception ex)
        {
            CSMain.Server.log.Error("GetItemFromRole       ：" + ex.Message);
        }
        return null;
    }

    /// <summary>
    /// 添加一个新物品到数据库
    /// </summary>
    /// <param name="item"></param>
    /// <param name="roleID"></param>
    /// <returns></returns>
    public static bool AddItem(List<_DBItem> dbItemList)
    {
        return false;
    }

    /// <summary>
    /// 更新物品信息
    /// </summary>
    /// <param name="dbItemList"></param>
    /// <returns></returns>
    public static bool UpdateItem(List<_DBItem> dbItemList)
    {
        return true;
    }

    /// <summary>
    /// 删除物品
    /// </summary>
    /// <param name="id">id</param>
    /// <param name="count">数量</param>
    /// <returns></returns>
    public static bool RmItem(int id,int count)
    {
        return true;
    }

    private static List<Item> BuildListItem(IList<_DBItem> dbItemList)
    {
        List<Item> itemList = new List<Item>();
        if(dbItemList==null)
        {
            return itemList;
        }
        //构造Item
        foreach (_DBItem dbItem in dbItemList)
        {
            Item item = new Item(dbItem);
            itemList.Add(item);
        }
        return itemList;
    }
}

