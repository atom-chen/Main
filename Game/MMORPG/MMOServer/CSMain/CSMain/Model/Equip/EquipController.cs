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
}

