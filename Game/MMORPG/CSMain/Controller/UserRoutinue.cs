using DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


class UserRoutinue:RoutinueBase
{
    public override void SetUp()
    {

    }
    public override ROUTINUE_CODE Code 
    {
        get { return ROUTINUE_CODE.USER; }
    }
    public override int UseResources
    {
        get { return 100; }
    }
    public static Dictionary<int, User> m_OnlineUser = new Dictionary<int, User>();//在线用户

    public static Queue<CG_PAK_BASE> pakCache = new Queue<CG_PAK_BASE>();
    public override void Tick()
    {
        while(pakCache.Count > 0)
        {
            CG_PAK_BASE pak = pakCache.Dequeue();
            User user = null;
            if (m_OnlineUser.TryGetValue(pak.SenderId, out user))
            {
                pak.Handle(user);
            }
        }
    }
    public UserRoutinue()
    {
        CSMain.Server.OnTeamDown += OnServerTeamDown;
    }

    public void OnUserLogin(User user)
    {
        m_OnlineUser.Add(user.Guid, user);
    }

    public void OnServerTeamDown()
    {
        //所有玩家下线 数据入库
        foreach(User user in m_OnlineUser.Values)
        {
            DownLine(user);
        }
    }

    //玩家登陆，登陆后将玩家的数据全部取出来
    public  void Login(int userId)
    {

        //Tools.SendMsgToDB()
        ////去数据库取
        //_DBUser dbUser = UserController.GetUserByID(loginUser.Guid);
        //if (dbUser != null)
        //{
        //    User user = new User(dbUser);
        //    if (user.PassWord.Equals(loginUser.PassWord))
        //    {
        //        m_OnlineUser.Add(loginUser.Guid, user);//上线玩家添加到集合
        //        return user.Guid;
        //    }
        //    else
        //    {
        //        return -1;
        //    }
        //}
        //else
        //{
        //    return -1;
        //}
    }

    //玩家下线
    public void DownLine(User user)
    {
        m_OnlineUser.Remove(user.Guid);
        //验证合法性 TODO
        //UserController.UpdateUser(new _DBUser(user));
    }

    //注册新用户
    public bool Register(User newUser)
    {
        _DBUser dbUser = null; //UserController.GetUserByUserName(newUser.UserName);
        if (dbUser != null)
        {
            return false;
        }
        dbUser = new _DBUser();
        dbUser.UserName = newUser.UserName;
        dbUser.Password = newUser.PassWord;
        dbUser.Guid = newUser.Guid;
        //UserController.InsertUser(dbUser);
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
            //_DBUser dbUser = UserController.GetUserByID(id);
            //if(dbUser!=null)
            //{
            //    return new User(dbUser);
            //}
        }
        return null;
    }

    /// <summary>
    /// 更新一个User的数据入库
    /// </summary>
    /// <param name="nUser"></param>
    private void UpdateUserInfo(User nUser)
    {
        //UserController.UpdateUser(new _DBUser(nUser));
    }
//-------------------------------MSG--------------------------------------
    public static void LoginSuccess(DM_UserConnectMsg msg,Connect conn)
    {
        User user = new User();
        user._Connect = conn;
        user.BuildFromDB(msg);
        m_OnlineUser.Add(user.Guid, user);
        //给玩家发送登录成功消息
        GC_ENTER_GAME_RET_PAK pak = new GC_ENTER_GAME_RET_PAK();
        pak._User = user;
        user.SendPak(pak);
    }

    public static void ReceiverMsg(DM_UserMsg msg)
    {
        User user = null;
        if(m_OnlineUser.TryGetValue(msg.User.Guid,out user))
        {
            user.BuildFromDB(msg);
        }
    }
    public static void ReceiverMsg(DM_RoleMsg msg)
    {

    }
    public static void ReceiveCGPak(CG_PAK_BASE pak)
    {
        pakCache.Enqueue(pak);
    }




}

