using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DB
{
  class RoleManager
  {
    private static RoleManager _Instance = new RoleManager();

    private RoleManager()
    {

    }
    public static RoleManager Instance
    {
      get
      {
        return _Instance;
      }
    }

    /// <summary>
    /// 获取属于id的所有角色
    /// </summary>
    /// <param name="Id">角色id</param>
    /// <returns></returns>
    public IList<_DBRole> GetRoleByUserID(int Id)
    {
      try
      {
        using (var session = NHibernateHelper.OpenSession())
        {
          using (var transction = session.BeginTransaction())
          {
            var servers = session.QueryOver<_DBRole>().Where(role => role.UserID == Id);
            transction.Commit();
            return servers.List();
          }
        }
      }
      catch (Exception ex)
      {
        CSMain.Server.log.Error("GetRoleByUserID       ："+ex.Message);
      }
      return null;
    }

    public IList<_DBRole> GetRoleByID(int Id)
    {
      try
      {
        using (var session = NHibernateHelper.OpenSession())
        {
          using (var transction = session.BeginTransaction())
          {
            var servers = session.QueryOver<_DBRole>().Where(role => role.ID == Id);
            transction.Commit();
            return servers.List();
          }
        }
      }
      catch (Exception ex)
      {
        CSMain.Server.log.Error("GetRoleByID       ：" + ex.Message);
      }
      return null;
    }

    /// <summary>
    /// 根据角色名称获取角色
    /// </summary>
    /// <param name="roleName"></param>
    /// <returns></returns>
    public _DBRole GetRoleByRoleName(string roleName)
    {
      try
      {
        using (var session = NHibernateHelper.OpenSession())
        {
          using (var transction = session.BeginTransaction())
          {
            var servers = session.QueryOver<_DBRole>().Where(role => role.Name.Equals(roleName));
            transction.Commit();
            if (servers.List().Count>0)
            {
              return servers.List()[0];
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
        CSMain.Server.log.Error("GetRoleByRoleName       ：" + ex.Message);
      }
      return null;
    }

    public void InsertRole(_DBRole role)
    {
      try
      {
        using (var session = NHibernateHelper.OpenSession())
        {
          using (var transction = session.BeginTransaction())
          {
            session.Save(role);
            transction.Commit();
          }
        }
      }
      catch (Exception ex)
      {
        CSMain.Server.log.Error("InsertRole       ：" + ex.Message);
      }
    }

    public void UpdateRole(_DBUser role)
    {
      try
      {
        using (var session = NHibernateHelper.OpenSession())
        {
          using (var transction = session.BeginTransaction())
          {
            session.Update(role);
            transction.Commit();
          }
        }
      }
      catch (Exception ex)
      {
        CSMain.Server.log.Error("UpdateRole       ：" + ex.Message);
      }
    }
  }
}


