using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
/*
 * User表的操作
 */

namespace DB
{
    class UserController
    {
        public static IList<_DBUser> GetAllUser()
        {
            try
            {
                using (var session = NHibernateHelper.OpenSession())
                {
                    using (var transction = session.BeginTransaction())
                    {
                        var users = session.QueryOver<_DBUser>();
                        transction.Commit();
                        return users.List();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return null;
        }

        public static _DBUser GetUserByID(int Guid)
        {
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
                            return user.List().First<_DBUser>();
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
                Console.WriteLine(ex.Message);
            }
            return null;

        }

        public static _DBUser GetUserByUserName(string UserName)
        {
            try
            {
                using (var session = NHibernateHelper.OpenSession())
                {
                    using (var transction = session.BeginTransaction())
                    {
                        var user = session.QueryOver<_DBUser>().Where(users => users.UserName == UserName);
                        if (user != null && user.List().Count > 0)
                        {
                            return user.List().First<_DBUser>();
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
                Console.WriteLine(ex.Message);
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
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
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
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
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
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public static IList<_DBUser> GetActiveUser()
        {
            try
            {
                using (var session = NHibernateHelper.OpenSession())
                {
                    using (var transction = session.BeginTransaction())
                    {
                        var users = session.QueryOver<_DBUser>();
                        transction.Commit();
                        return users.List();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return null;
        }
    }
}

