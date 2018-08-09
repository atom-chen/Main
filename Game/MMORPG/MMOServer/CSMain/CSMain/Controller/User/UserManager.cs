using DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


class UserManager
{
    private static Dictionary<int, User> m_OnlineUser = new Dictionary<int, User>();//在线用户


    static UserManager()
    {
        CSMain.Server.OnTeamDown += OnServerTeamDown;
    }

    public static void OnServerTeamDown()
    {
        //所有玩家下线 数据入库
        foreach(User user in m_OnlineUser.Values)
        {
            DownLine(user);
        }
    }

    //玩家登陆
    public  static int Login(User loginUser)
    {
        //去数据库取
        _DBUser dbUser = UserController.GetUserByID(loginUser.Guid);
        if (dbUser != null)
        {
            User user = new User(dbUser);
            if (user.PassWord.Equals(loginUser.PassWord))
            {
                m_OnlineUser.Add(loginUser.Guid, user);//上线玩家添加到集合
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
    public static void DownLine(User user)
    {
        //验证合法性 TODO
        UserController.UpdateUser(new _DBUser(user));
        m_OnlineUser.Remove(user.Guid);
    }

    //注册新用户
    public static bool Register(User newUser)
    {
        _DBUser dbUser = UserController.GetUserByUserName(newUser.UserName);
        if (dbUser != null)
        {
            return false;
        }
        dbUser = new _DBUser();
        dbUser.UserName = newUser.UserName;
        dbUser.Password = newUser.PassWord;
        dbUser.Guid = newUser.Guid;
        UserController.InsertUser(dbUser);
        m_OnlineUser.Add(newUser.Guid, newUser);
        return true;
    }

    /// <summary>
    /// 获取一个User
    /// </summary>
    /// <param name="id">id</param>
    /// <returns></returns>
    public User GetUser(int id)
    {
        User user;
        if(m_OnlineUser.TryGetValue(id,out user))
        {
            return user;
        }
        else
        {
            //去数据库取
            _DBUser dbUser = UserController.GetUserByID(id);
            if(dbUser!=null)
            {
                return new User(dbUser);
            }
        }
        return null;
    }

    /// <summary>
    /// 更新一个User的数据入库
    /// </summary>
    /// <param name="nUser"></param>
    public void UpdateUserInfo(User nUser)
    {
        UserController.UpdateUser(new _DBUser(nUser));
    }
}

