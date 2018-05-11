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
  private Dictionary<int, Role> m_ActiveRolesDic1 = new Dictionary<int, Role>();//所属UserID，角色信息
  private Dictionary<int, Role> m_ActiveRolesDic2 = new Dictionary<int, Role>();//所属UserID，角色信息

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
    if(m_ActiveRolesDic1.TryGetValue(userId,out role1))
    {
      result.Add(role1);
      //尝试获取第二个角色
      Role role2=null;
      if(m_ActiveRolesDic2.TryGetValue(userId,out role2))
      {
        result.Add(role2);
      }
      return result;
    }
    //去数据库找
    else
    {
      IList<DB._DBRole> dbRoles=DB.RoleManager.Instance.GetRoleByUserID(userId);
      if(dbRoles!=null)
      {
        foreach(DB._DBRole item in dbRoles)
        {
          role1 = new Role(item);
          result.Add(role1);
          AddRoleToDic(role1);
        }
      }
      return result;
    }
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
    AddRoleToDic(newRole);
    return true;
  }


  /// <summary>
  /// 一个玩家是否还能再创建角色
  /// </summary>
  /// <param name="userId">玩家ID</param>
  /// <returns>布尔</returns>
  private bool IsUserCanCreateRole(int userId)
  {
    //内存中已有两个角色
    if(m_ActiveRolesDic1.ContainsKey(userId))
    {
      if(m_ActiveRolesDic2.ContainsKey(userId))
      {
        return false;
      }
      else
      {
        return true;
      }
    }
    //内存中没有这个人，就去数据库找
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

  //添加角色到集合
  private void AddRoleToDic(Role role)
  {
    if (!m_ActiveRolesDic1.ContainsKey(role.UserID))
    {
      m_ActiveRolesDic1.Add(role.UserID, role);
    }
    else if (!m_ActiveRolesDic2.ContainsKey(role.UserID))
    {
      m_ActiveRolesDic2.Add(role.UserID, role);
    }
    else
    {
      throw new Exception("Role数据异常");
    }
  }

  
}

