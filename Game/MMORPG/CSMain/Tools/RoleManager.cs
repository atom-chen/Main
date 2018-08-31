using CSMain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


class RoleManager
{

    private static Dictionary<int, Role> m_OnlineRoles = new Dictionary<int, Role>();      //当前在线角色 所属UserID，角色信息

    static RoleManager()
    {
        Server.OnTeamDown += OnServerTeamDown;
    }
    public static void OnServerTeamDown()
    {
        foreach(Role role in m_OnlineRoles.Values)
        {
            RoleDownLine(role);                       //让所有角色下线
        }
    }

    //给所有玩家发送消息包
    public static void SendMessageToAllRoles()
    {
        foreach (Role role in m_OnlineRoles.Values)
        {


        }
    }

    /// <summary>
    /// 获取某个玩家的所有角色
    /// </summary>
    /// <param name="userId">玩家ID</param>
    /// <returns>角色List</returns>
    public static List<Role> GetUserAllRole(int userId)
    {
        List<Role> result = new List<Role>();
        //Role role1 = null;
        //Tools.SendMsgToDB()
        IList<DB._DBRole> dbRoles = null; //DB.RoleController.GetRoleByUserID(userId);
        if (dbRoles != null)
        {
            //foreach (DB._DBRole item in dbRoles)
            //{
            //    role1 = new Role(item);
            //    result.Add(role1);
            //}
        }
        return result;
    }

    //创建角色
    public static bool CreateRole(Role newRole)
    {
        //是否有足够的槽
        if (!IsUserCanCreateRole(newRole.userID))
        {
            return false;
        }
        //是否重名
        DB._DBRole dbRole = null; //DB.RoleController.GetRoleByRoleName(newRole.name);
        if (dbRole != null)
        {
            return false;
        }
        dbRole = new DB._DBRole(newRole);
        //DB.RoleController.InsertRole(dbRole);
        return true;
    }


    /// <summary>
    /// 一个玩家是否还能再创建角色
    /// </summary>
    /// <param name="userId">玩家ID</param>
    /// <returns>布尔</returns>
    private static bool IsUserCanCreateRole(int userId)
    {
        IList<DB._DBRole> dbRoleList = null; //DB.RoleController.GetRoleByUserID(userId);
        if (dbRoleList == null || dbRoleList.Count <= 1)
        {
            return true;
        }
        else
        {
            return false;
        }
    }


    //更新角色信息
    public static bool UpdateRoleInfo(Role role)
    {
        Role listRole = null;
        m_OnlineRoles.TryGetValue(role.userID, out listRole);
        if (listRole != null)
        {
            if (listRole.CompareToRole(role))
            {
                listRole = role;//允许更新
                return true;
            }
            else
            {
                LogManager.Error(string.Format("玩家ID={0},角色ID={1} 和服务器数据不一致", role.userID, role.id));
                return false;
            }
        }
        else
        {
            return false;
        }
    }


    //角色上线
    public static Role RoleOnline(Role role)
    {
        if (!m_OnlineRoles.ContainsKey(role.userID))
        {
            //验证数据一致性
            DB._DBRole dbRole = null; //DB.RoleController.GetRoleByID(role.id);
            if (role.CompareToDB(dbRole))
            {
               // role = BuildRole(dbRole);
                m_OnlineRoles.Add(role.userID, role);                          //允许上线
                return role;
            }
            else
            {
                LogManager.Debug(role.userID + "有作弊嫌疑，角色信息被修改");
                return null;
            }
        }
        else
        {
            //如果集合中有，则让该角色下线
            RoleDownLine(role);
            return null;
        }
    }


    //角色下线
    public static void RoleDownLine(Role role)
    {
        //1数据入库
        if (UpdateRoleInfo(role))
        {
            //role信息
            DB._DBRole db = BuildDBRole(role);
            //DB.RoleController.UpdateRole(db);
            m_OnlineRoles.Remove(role.userID);
            //物品信息入库
            SaveBag2DB(role);
            
        }
        else
        {
            LogManager.Error(string.Format("玩家ID={0},角色ID={1} 无法正确下线", role.userID, role.id));
        }

    }

    //改名
    public static void ReName(Role role)
    {

    }

    //根据DB信息构建角色内存信息
    //private static Role BuildRole(DB._DBRole db)
    //{
    //    //Role role = new Role(db);
    //    //role.Recover(db);                                           //体力回复
    //   // role.BuildBag(BagController.GetItemFromRole(db.ID));             //填充背包

    //    //装备：先填充身上穿的
    //    //List<Equip> equipList = null;//EquipController.GetEquipFromRole(role.id);
    //    //foreach(Equip equip in equipList)
    //    //{
    //    //    if(db.IsEquip(equip.guid))
    //    //    {
    //    //        role.FuncEquip(equip);
    //    //        equipList.Remove(equip);
    //    //    }
    //    //}
    //    //role.BuildEquipBag(equipList);                              //剩下的放入到装备背包

    //    //return role;
    //}

    private static DB._DBRole BuildDBRole(Role role)
    {
        DB._DBRole dbRole = new DB._DBRole(role);
        DateTime now = DateTime.Now;
        dbRole.LastDownLine = now.ToString("yyyy-MM-dd HH:mm:ss");//将当前时间作为下线时间
        LogManager.Debug("角色{0}在{1}下线", role.name, dbRole.LastDownLine);
        return dbRole;
    }

    private static void SaveBag2DB(Role role)
    {
        //BagController.UpdateItem(role.bag.GetList(),role.id);         //物品信息入库
        //EquipController.UpdateEquip(role.equipInfo.GetArray(), role.id);  //装备背包入库
    }

}

