using CSMain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


class RoleController
{
  private static RoleController _Instance=new RoleController();
  public static RoleController Instance
  {
    get
    {
      return _Instance;
    }
  }
  private RoleController()
  {

  }



  //活跃玩家应该保证其所有角色都在dic中，不活跃玩家应该同步清理所有数据
  private Dictionary<int, Role> m_OnlineRoles = new Dictionary<int, Role>();//所属UserID，角色信息

  /// <summary>
  /// 获取某个玩家的所有角色
  /// </summary>
  /// <param name="userId">玩家ID</param>
  /// <returns>角色List</returns>
  public List<Role> GetUserAllRole(int userId)
  {
    List<Role> result = new List<Role>();
    //先在字典里找
    Role role1 = null;
    IList<DB._DBRole> dbRoles = DB.RoleManager.Instance.GetRoleByUserID(userId);
    if (dbRoles != null)
    {
      foreach (DB._DBRole item in dbRoles)
      {
        role1 = new Role(item);
        result.Add(role1);
      }
    }
    return result;
  }

  //创建角色
  public bool CreateRole(Role newRole)
  {
    //是否有足够的槽
    if(!IsUserCanCreateRole(newRole.UserID))
    {
      return false;
    }
    //是否重名
    DB._DBRole dbRole = DB.RoleManager.Instance.GetRoleByRoleName(newRole.Name);
    if (dbRole != null)
    {
      return false;
    }
    dbRole = new DB._DBRole(newRole);
    DB.RoleManager.Instance.InsertRole(dbRole);
    return true;
  }


  /// <summary>
  /// 一个玩家是否还能再创建角色
  /// </summary>
  /// <param name="userId">玩家ID</param>
  /// <returns>布尔</returns>
  private bool IsUserCanCreateRole(int userId)
  {
    IList<DB._DBRole> dbRoleList = DB.RoleManager.Instance.GetRoleByUserID(userId);
    if (dbRoleList == null || dbRoleList.Count<=1)
    {
      return true;
    }
    else
    {
      return false;
    }
  }


  //更新角色信息
  public bool UpdateRoleInfo(Role role)
  {
    Role listRole=null;
    m_OnlineRoles.TryGetValue(role.UserID, out listRole);
    if(listRole!=null)
    {
      if (listRole.CompareToRole(role))
      {
        listRole=role;//允许更新
        return true;
      }
      else
      {
        CSMain.Server.log.Error(string.Format("玩家ID={0},角色ID={1} 和服务器数据不一致", role.UserID, role.ID));
        return false;
      }
    }
    else
    {
      return false;
    }
  }


  //角色上线
  public Role RoleOnline(Role role)
  {
    if (!m_OnlineRoles.ContainsKey(role.UserID))
    {
      //验证数据一致性
      DB._DBRole dbRole = DB.RoleManager.Instance.GetRoleByID(role.ID);
      if(role.CompareToDB(dbRole))
      {
        role = new Role(dbRole);
        role.Recover(dbRole);
        m_OnlineRoles.Add(role.UserID, role);//允许上线
        return role;
      }
      else
      {
        CSMain.Server.log.Debug(role.UserID + "有作弊嫌疑，角色信息被修改");
        return null;
      }
    }
    else
    {
        //如果集合中有，则不让该角色下线
        return null;
    }
  }


  //角色下线
  public void RoleDownLine(Role role)
  {
    //1数据入库
    if (UpdateRoleInfo(role))
    {
      DB._DBRole dbRole = new DB._DBRole(role);
      DateTime now = DateTime.Now;
      dbRole.LastDownLine = now.ToString("yyyy-MM-dd HH:mm:ss");//将当前时间作为下线时间
      CSMain.Server.log.DebugFormat("角色{0}在{1}下线", role.Name,dbRole.LastDownLine);
      DB.RoleManager.Instance.UpdateRole(dbRole);
      //2从集合中移除
      m_OnlineRoles.Remove(role.UserID);
    }
    else
    {
      CSMain.Server.log.Error(string.Format("玩家ID={0},角色ID={1} 无法正确下线", role.UserID, role.ID));
    }

  }

  //改名
  public void ReName(Role role)
  {

  }
}

