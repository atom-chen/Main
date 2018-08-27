using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DB;


public class EquipController
{
    public static List<Equip> GetEquipFromRole(int roleId)
    {
        try
        {
            using (var session = NHibernateHelper.OpenSession())
            {
                using (var transction = session.BeginTransaction())
                {
                    var servers = session.QueryOver<_DBEquip>().Where(equip => equip.roleId == roleId);   
                    transction.Commit();
                    List<Equip> ret = IListToList(servers.List());
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
    
    //适配为内存模型
    private static List<Equip> IListToList(IList<_DBEquip> equipList)
    {
        List<Equip> ret = new List<Equip>();
        foreach(_DBEquip db in equipList)
        {
            ret.Add(new Equip(db));
        }
        return ret;
    }

    //更新装备信息
    public static void UpdateEquip(Equip equip,int roleId)
    {
        try
        {
            using (var session = NHibernateHelper.OpenSession())
            {
                using (var transction = session.BeginTransaction())
                {
                    session.Update(new _DBEquip(equip,roleId));
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
    public static void UpdateEquip(Equip[] equipArr,int roleId)
    {
        try
        {
            using (var session = NHibernateHelper.OpenSession())
            {
                using (var transction = session.BeginTransaction())
                {
                    foreach (Equip equip in equipArr)
                    {
                        session.Update(new _DBEquip(equip,roleId));
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
    public static void AddEquip(Equip equip, int roleID)
    {
        try
        {
            using (var session = NHibernateHelper.OpenSession())
            {
                using (var transction = session.BeginTransaction())
                {
                    session.Save(new _DBEquip(equip, roleID));
                    transction.Commit();
                }
            }
        }
        catch (Exception ex)
        {
            LogManager.Error("AddItem       ：" + ex.Message);
        }
    }
}

