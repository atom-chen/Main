using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DB
{
    class RoleController
    {
        /// <summary>
        /// 获取属于id的所有角色
        /// </summary>
        /// <param name="Id">角色id</param>
        /// <returns></returns>
        public static IList<_DBRole> GetRoleByUserID(int Id)
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
                LogManager.Error("GetRoleByUserID       ：" + ex.Message);
            }
            return null;
        }

        public static _DBRole GetRoleByID(int Id)
        {
            try
            {
                using (var session = NHibernateHelper.OpenSession())
                {
                    using (var transction = session.BeginTransaction())
                    {
                        var servers = session.QueryOver<_DBRole>().Where(role => role.ID == Id);
                        transction.Commit();
                        if (servers.List().Count >= 1)
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
                LogManager.Error("GetRoleByID       ：" + ex.Message);
            }
            return null;
        }

        /// <summary>
        /// 根据角色名称获取角色
        /// </summary>
        /// <param name="roleName"></param>
        /// <returns></returns>
        public static _DBRole GetRoleByRoleName(string roleName)
        {
            try
            {
                using (var session = NHibernateHelper.OpenSession())
                {
                    using (var transction = session.BeginTransaction())
                    {
                        var servers = session.QueryOver<_DBRole>().Where(role => role.Name == roleName);
                        transction.Commit();
                        if (servers.List().Count > 0)
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
                LogManager.Error("GetRoleByRoleName       ：" + ex.Message);
            }
            return null;
        }

        public static void InsertRole(_DBRole role)
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
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Error("UpdateRole       ：" + ex.Message);
            }
        }
    }
}


