using DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


class UserController
{
    private static UserController _Instance = new UserController();
    public static UserController Instance
    {
        get
        {
            return _Instance;
        }
    }
    private UserController()
    {
        m_OnlineUser = new Dictionary<string, User>();
    }
    private Dictionary<string, User> m_OnlineUser;//在线用户

    //玩家登陆
    public int Login(User loginUser)
    {
        User user;
        //去数据库取
        _DBUser dbUser = UserManager.Instance.GetUserByUserName(loginUser.UserName);
        if (dbUser != null)
        {
            user = new User(dbUser);
            if (user.PassWord.Equals(loginUser.PassWord))
            {
                m_OnlineUser.Add(loginUser.UserName, user);//上线玩家添加到集合
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

    //玩家下线
    public void DonwLine(User user)
    {
        //验证合法性 TODO

        UserManager.Instance.UpdateUser(new _DBUser(user));
        m_OnlineUser.Remove(user.UserName);
    }

    //失败原因：用户名已存在
    public bool Register(User newUser)
    {
        _DBUser dbUser = UserManager.Instance.GetUserByUserName(newUser.UserName);
        if (dbUser != null)
        {
            return false;
        }
        dbUser = new _DBUser();
        dbUser.UserName = newUser.UserName;
        dbUser.Password = newUser.PassWord;
        dbUser.Guid = newUser.Guid;
        UserManager.Instance.InsertUser(dbUser);
        m_OnlineUser.Add(newUser.UserName, newUser);
        return true;
    }

}

