﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DB;

public class BagController
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
            LogManager.Error("GetItemFromRole       ：" + ex.Message);
        }
        return null;
    }

    /// <summary>
    /// 添加新物品到数据库
    /// </summary>
    public static void AddItem(List<_DBItem> dbItemList)
    {
        try
        {
            using (var session = NHibernateHelper.OpenSession())
            {
                using (var transction = session.BeginTransaction())
                {
                    foreach (_DBItem dbItem in dbItemList)
                    {
                        session.Save(dbItem);
                    }
                    transction.Commit();
                }
            }
        }
        catch (Exception ex)
        {
            LogManager.Error("AddItem       ：" + ex.Message);
        }
    }

    /// <summary>
    /// 添加新物品到数据库
    /// </summary>
    public static void AddItem(List<Item> itemList, int roleID)
    {
        try
        {
            using (var session = NHibernateHelper.OpenSession())
            {
                using (var transction = session.BeginTransaction())
                {
                    foreach (Item item in itemList)
                    {
                        session.Save(new _DBItem(item, roleID));
                    }
                    transction.Commit();
                }
            }
        }
        catch (Exception ex)
        {
            LogManager.Error("AddItem       ：" + ex.Message);
        }
    }

    /// <summary>
    /// 更新物品信息
    /// </summary>
    /// <param name="dbItemList"></param>
    /// <returns></returns>
    public static void UpdateItem(List<_DBItem> dbItemList)
    {
        try
        {
            using (var session = NHibernateHelper.OpenSession())
            {
                using (var transction = session.BeginTransaction())
                {
                    foreach(_DBItem db in dbItemList)
                    {
                        session.Update(db);
                    }
                    transction.Commit();
                }
            }
        }
        catch (Exception ex)
        {
            LogManager.Error("UpdateRole       ：" + ex.Message);
        }
    }

    /// <summary>
    /// 更新物品信息
    /// </summary>
    /// <param name="dbItemList"></param>
    /// <returns></returns>
    public static void UpdateItem(List<Item> itemList,int roleId)
    {
        try
        {
            using (var session = NHibernateHelper.OpenSession())
            {
                using (var transction = session.BeginTransaction())
                {
                    foreach (Item item in itemList)
                    {
                        session.Update(new _DBItem(item,roleId));
                    }
                    transction.Commit();
                }
            }
        }
        catch (Exception ex)
        {
            LogManager.Error("UpdateRole       ：" + ex.Message);
        }
    }

    /// <summary>
    /// 删除物品
    /// </summary>
    /// <param name="id">id</param>
    /// <param name="count">数量</param>
    /// <returns></returns>
    public static void RmItem(int id, int count)
    {
    }


    //将IList转换为List
    private static List<Item> BuildListItem(IList<_DBItem> dbItemList)
    {
        List<Item> itemList = new List<Item>();
        if (dbItemList == null)
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

