using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DB;

class RoleController:ControllerBase
{
    
    private static Dictionary<int, Cache<List<_DBRole>>> m_ActiveDic = new Dictionary<int, Cache<List<_DBRole>>>();
    /// <summary>
    /// 获取属于id的所有角色
    /// </summary>
    /// <param name="Id">Userid</param>
    /// <returns></returns>
    public static IList<_DBRole> GetRoleByUserID(int Id)
    {
        Cache<List<_DBRole>> cache;
        if (m_ActiveDic.TryGetValue(Id, out cache))
        {
            cache.lastQuery = DateTime.Now;
            return cache.data;
        }
        try
        {
            using (NHibernate.ISession session = NHibernateHelper.OpenSession())
            {
                using (NHibernate.ITransaction transction = session.BeginTransaction())
                {
                    var servers = session.QueryOver<_DBRole>().Where(role => role.UserID == Id);
                    transction.Commit();
                    IList<_DBRole> ret = servers.List();
                    //缓存
                    cache = new Cache<List<_DBRole>>();
                    cache.data = ret as List<_DBRole>;
                    cache.lastQuery = DateTime.Now;
                    m_ActiveDic.Add(Id, cache);
                    return ret;
                }
            }
        }
        catch (Exception ex)
        {
            LogManager.Error("GetRoleByUserID       ：" + ex.Message);
        }
        return null;
    }

    //public static _DBRole GetRoleByID(int Id)
    //{
    //    try
    //    {
    //        using (var session = NHibernateHelper.OpenSession())
    //        {
    //            using (var transction = session.BeginTransaction())
    //            {
    //                var servers = session.QueryOver<_DBRole>().Where(role => role.ID == Id);
    //                transction.Commit();
    //                if (servers.List().Count >= 1)
    //                {
    //                    _DBRole ret = servers.List().First<_DBRole>();
    //                    return ret;
    //                }
    //                else
    //                {
    //                    return null;
    //                }

    //            }
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        LogManager.Error("GetRoleByID       ：" + ex.Message);
    //    }
    //    return null;
    //}

    ///// <summary>
    ///// 根据角色名称获取角色
    ///// </summary>
    ///// <param name="roleName"></param>
    ///// <returns></returns>
    //public static _DBRole GetRoleByRoleName(string roleName)
    //{
    //    try
    //    {
    //        using (var session = NHibernateHelper.OpenSession())
    //        {
    //            using (var transction = session.BeginTransaction())
    //            {
    //                var servers = session.QueryOver<_DBRole>().Where(role => role.Name == roleName);
    //                transction.Commit();
    //                if (servers.List().Count > 0)
    //                {
    //                    return servers.List()[0];
    //                }
    //                else
    //                {
    //                    return null;
    //                }
    //            }
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        LogManager.Error("GetRoleByRoleName       ：" + ex.Message);
    //    }
    //    return null;
    //}

    public static void InsertRole(_DBRole role)
    {
        try
        {
            using (var session = NHibernateHelper.OpenSession())
            {
                using (var transction = session.BeginTransaction())
                {
                    object obj = session.Save(role);
                    transction.Commit();
                    Cache<List<_DBRole>> cache;
                    role = obj as _DBRole;
                    if(role !=null)
                    {
                        //如果集合里有这个User的角色信息
                        if (m_ActiveDic.TryGetValue(role.UserID, out cache))
                        {
                            cache.data.Add(role);
                        }
                        else
                        {
                            cache.data.Add(role);
                            m_ActiveDic.Add(role.UserID, cache);
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            LogManager.Error("InsertRole       ：" + ex.Message);
        }
    }

    public static void UpdateRole(_DBRole role)
    {
        try
        {
            using (var session = NHibernateHelper.OpenSession())
            {
                using (var transction = session.BeginTransaction())
                {
                    session.Update(role);
                    transction.Commit();

                    //在内存数据中同步更新
                    Cache<List<_DBRole>> cache;
                    if (m_ActiveDic.TryGetValue(role.UserID, out cache))
                    {
                        cache.lastQuery = DateTime.Now;
                        for(int i=0;i<cache.data.Count;i++)
                        {
                            if(cache.data[i].ID == role.ID)
                            {
                                cache.data[i] = role;
                                break;
                            }
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            LogManager.Error("UpdateRole       ：" + ex.Message);
        }
    }

    public override void Tick()
    {

    }

    public override void Clear()
    {

    }
}



