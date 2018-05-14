using DB;
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
  private Dictionary<string,User> m_ActiveUser;//近7日活跃玩家

  public int Login(User loginUser)
  {
    User user;
    //先在字典里找
    m_ActiveUser.TryGetValue(loginUser.UserName, out user);
    if(user!=null)
    {
      if (user.PassWord.Equals(loginUser.PassWord))  //直接存储密文
      {
        return user.Guid;
      }
      else
      {
        return -1;
      }
    }
    else
    {
      //去数据库取
      _DBUser dbUser = UserManager.Instance.GetUserByUserName(loginUser.UserName);
      if(dbUser!=null)
      {
        user = new User(dbUser);
        if (user.PassWord.Equals(loginUser.PassWord))
        {
          m_ActiveUser.Add(loginUser.UserName, user);
          return user.Guid;
        }
        else
        {
          return -1;
        }
      }
      else
      {
        return -1;
      }
    }
  }

  //失败原因：用户名已存在
  public bool Register(User newUser)
  {
    _DBUser dbUser = UserManager.Instance.GetUserByUserName(newUser.UserName);
    if(dbUser!=null)
    {
      return false;
    }
    dbUser = new _DBUser();
    dbUser.UserName=newUser.UserName;
    dbUser.Password=newUser.PassWord;
    dbUser.Guid=newUser.Guid;
    UserManager.Instance.InsertUser(dbUser);
    m_ActiveUser.Add(newUser.UserName, newUser);
    return true;
  }
  
}

