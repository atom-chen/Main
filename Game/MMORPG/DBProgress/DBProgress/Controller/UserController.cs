using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DB;
/*
 * User表的操作
 */
struct Cache<T>where T:class
{
    public T data;
    public DateTime lastQuery;
    public Cache()
    {
        data = null;
        lastQuery = DateTime.Now;
    }
    public Cache(T user, DateTime time)
    {
        this.data = user;
        this.lastQuery = time;
    }
}

class UserController:ControllerBase
{
    private static Dictionary<int, Cache<_DBUser>> m_ActiveDic = new Dictionary<int, Cache<_DBUser>>();

    


    public static _DBUser GetUserByID(int Guid)
    {
        //如果缓存里有，则直接把缓存数据返回
        Cache<_DBUser> data;
        if (m_ActiveDic.TryGetValue(Guid, out data))
        {
            data.lastQuery = DateTime.Now;
            return data.data;
        }
        try
        {
            using (var session = NHibernateHelper.OpenSession())
            {
                using (var transction = session.BeginTransaction())
                {
                    var user = session.QueryOver<_DBUser>().Where(users => users.Guid == Guid);
                    transction.Commit();
                    if (user.List().Count > 0)
                    {
                        //如果是首次查询
                        _DBUser ret = user.List().First<_DBUser>();
                        m_ActiveDic.Add(ret.Guid, new Cache<_DBUser>(ret, DateTime.Now));
                    }
                    else
                    {
                        return null;
                    }
                }
            }
        }
        catch (Exception ex)
        {
            LogManager.Error(ex.Message);
        }
        return null;

    }

    public static _DBUser GetUserByUserName(string UserName)
    {
        for (int i = 0; i < m_ActiveDic.Count;i++ )
        {
            Cache<_DBUser> user = m_ActiveDic[i];
            if (user.data.UserName == UserName)
            {
                user.lastQuery = DateTime.Now;
                return user.data;
            }
        }
        try
        {
            using (var session = NHibernateHelper.OpenSession())
            {
                using (var transction = session.BeginTransaction())
                {
                    var user = session.QueryOver<_DBUser>().Where(users => users.UserName == UserName);
                    if (user.List().Count > 0)
                    {
                        //如果是首次查询
                        _DBUser ret = user.List().First<_DBUser>();
                        m_ActiveDic.Add(ret.Guid, new Cache<_DBUser>(ret, DateTime.Now));
                    }
                    else
                    {
                        return null;
                    }

                }
            }
        }
        catch (Exception ex)
        {
            LogManager.Error(ex.Message);
        }
        return null;
    }


    public static void InsertUser(_DBUser user)
    {
        try
        {
            using (var session = NHibernateHelper.OpenSession())
            {
                using (var transction = session.BeginTransaction())
                {
                    session.Save(user);
                    transction.Commit();
                    Cache<_DBUser> cache = new Cache<_DBUser>(user, DateTime.Now);
                    m_ActiveDic.Add(user.Guid, cache);
                }
            }
        }
        catch (Exception ex)
        {
            LogManager.Error(ex.Message);
        }
    }


    public static void DeleteUserByID(int ID)
    {
        try
        {
            using (var session = NHibernateHelper.OpenSession())
            {
                using (var transction = session.BeginTransaction())
                {
                    _DBUser deUser = new _DBUser();
                    deUser.Guid = ID;
                    session.Delete(deUser);
                    transction.Commit();
                    m_ActiveDic.Remove(ID);
                }
            }
        }
        catch (Exception ex)
        {
            LogManager.Error(ex.Message);
        }
    }

    public static void UpdateUser(_DBUser user)
    {
        try
        {
            using (var session = NHibernateHelper.OpenSession())
            {
                using (var transction = session.BeginTransaction())
                {
                    session.Update(user);
                    transction.Commit();
                    Cache<_DBUser> cache;
                    if(m_ActiveDic.TryGetValue(user.Guid,out cache))
                    {
                        cache.data = user;
                    }
                }
            }
        }
        catch (Exception ex)
        {
            LogManager.Error(ex.Message);
        }
    }

    public override void Tick()
    {

    }

    public override void Clear()
    {

    }
}


