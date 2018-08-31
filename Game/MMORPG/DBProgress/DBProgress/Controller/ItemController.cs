using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DB;

public class BagController:ControllerBase
{
    private static Dictionary<int, Cache<List<_DBItem>>> m_ActiveDic = new Dictionary<int, Cache<List<_DBItem>>>();
    /// <summary>
    /// 获取属于role的所有物品，返回
    /// </summary>
    /// <param name="roleID">角色ID</param>
    /// <returns>物品list</returns>
    public static IList<_DBItem> GetItemFromRole(int roleID)
    {
        Cache<List<_DBItem>> cache;
        if (m_ActiveDic.TryGetValue(roleID, out cache))
        {
            cache.lastQuery = DateTime.Now;
            return cache.data;
        }
        try
        {
            using (var session = NHibernateHelper.OpenSession())
            {
                using (var transction = session.BeginTransaction())
                {
                    var servers = session.QueryOver<_DBItem>().Where(item => item.roleID == roleID);
                    transction.Commit();
                    IList<_DBItem> ret = servers.List();

                    //缓存
                    cache = new Cache<List<_DBItem>>();
                    cache.data = ret as List<_DBItem>;
                    cache.lastQuery = DateTime.Now;
                    m_ActiveDic.Add(roleID, cache);
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
    /// 更新物品
    /// </summary>
    public static void UpdateItem(List<_DBItem> dbItemList)
    {
        try
        {
            using (var session = NHibernateHelper.OpenSession())
            {
                using (var transction = session.BeginTransaction())
                {
                    foreach (_DBItem dbItem in dbItemList)
                    {
                        //更新数据：非active玩家不支持更新
                        Cache<List<_DBItem>> cache;
                        if(m_ActiveDic.TryGetValue(dbItem.roleID,out cache))
                        {
                            bool success = false;
                            for(int i=0;i<cache.data.Count ;i++)
                            {
                                if(cache.data[i].tabID == dbItemList[i].tabID)
                                {
                                    success = true;
                                    //如果是更新数量
                                    if (dbItemList[i].count > 0)
                                    {
                                        cache.data[i].count = dbItemList[i].count;
                                        session.Update(dbItem);
                                    }
                                    //数量小于0 直接删除
                                    else
                                    {
                                        cache.data.RemoveAt(i);
                                        session.Delete(dbItem);
                                    }
                                    break;
                                }
                            }
                            //如果没有 则add
                            if(!success)
                            {
                                cache.data.Add(dbItem);
                                session.Save(dbItem);
                            }
                            cache.lastQuery = DateTime.Now;
                        }
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

    public override void Tick()
    {
    }

    public override void Clear()
    {

    }
}

