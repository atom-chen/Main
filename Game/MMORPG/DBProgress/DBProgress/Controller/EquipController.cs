using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DB;


public class EquipController:ControllerBase
{
    private static Dictionary<int, Cache<List<_DBEquip>>> m_ActiveDic = new Dictionary<int, Cache<List<_DBEquip>>>();
    public static IList<_DBEquip> GetEquipFromRole(int roleId)
    {
        //如果内存里有
        Cache<List<_DBEquip>> cache;
        if (m_ActiveDic.TryGetValue(roleId, out cache))
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
                    var servers = session.QueryOver<_DBEquip>().Where(equip => equip.roleId == roleId);   
                    transction.Commit();
                    IList<_DBEquip> ret = servers.List();

                    //缓存
                    cache = new Cache<List<_DBEquip>>();
                    cache.data = ret as List<_DBEquip>;
                    cache.lastQuery = DateTime.Now;
                    m_ActiveDic.Add(roleId, cache);
                    return ret;
                }
            }
        }
        catch (Exception ex)
        {
            LogManager.Error("GetEquipFromUser       ：" + ex.Message);
        }
        return null;
    }
    

    //更新装备
    private static void UpdateEquip(_DBEquip equip)
    {
        try
        {
            using (var session = NHibernateHelper.OpenSession())
            {
                using (var transction = session.BeginTransaction())
                {
                    session.Update(equip);
                    transction.Commit();
                }
            }
        }
        catch (Exception ex)
        {
            LogManager.Error("UpdateRole       ：" + ex.Message);
        }
    }

    //更新身上穿的装备
    public static void UpdateEquip(_DBEquip[] equipArr)
    {
        try
        {
            using (var session = NHibernateHelper.OpenSession())
            {
                using (var transction = session.BeginTransaction())
                {
                    foreach (_DBEquip equip in equipArr)
                    {
                        session.Update(equip);
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

    //往数据库写入一个新装备
    private static void AddEquip(_DBEquip equip)
    {
        try
        {
            using (var session = NHibernateHelper.OpenSession())
            {
                using (var transction = session.BeginTransaction())
                {
                    session.Save(equip);
                    transction.Commit();
                }
            }
        }
        catch (Exception ex)
        {
            LogManager.Error("AddItem       ：" + ex.Message);
        }
    }

    public  override void Tick()
    {

    }

    public  override void Clear()
    {

    }
}

