using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


class UserController
{
  private static UserController _Instance=new UserController();
  public static UserController Instance
  {
    get
    {
      return _Instance;
    }
  }
  private UserController()
  {
    //获取最近7日上线过的玩家，将信息放到内存
    m_ActiveUser = new Dictionary<string, User>();
    IList<_DBUser> dbUsers = UserManager.Instance.GetActiveUser();
    if(dbUsers!=null)
    {
      foreach(var item in dbUsers)
      {
        m_ActiveUser.Add(item.UserName,new User(item));
      }
    }
  }
  private Dictionary<string,User> m_ActiveUser;

  public User Login(string userName,string passWord)
  {
    User user;
    //先在字典里找
    m_ActiveUser.TryGetValue(userName, out user);
    if(user!=null)
    {
      if (user.PassWord.Equals(passWord))
      {
        return user;
      }
      else
      {
        return null;
      }
    }
    else
    {
      //去数据库取
      _DBUser dbUser = UserManager.Instance.GetUserByUserName(userName);
      if(dbUser!=null)
      {
        user = new User(dbUser);
        m_ActiveUser.Add(userName, user);
        if(user.PassWord.Equals(passWord))
        {
          return user;
        }
        else
        {
          return null;
        }
      }
      else
      {
        return null;
      }
    }
  }
  
}

