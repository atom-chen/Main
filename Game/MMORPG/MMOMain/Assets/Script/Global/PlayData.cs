using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
/// <summary>
/// 玩家数据
/// </summary>
/// 

public class PlayData
{
  private static User m_User;
  private static ServerProperty m_Server;
  private static List<Role> m_RoleList;
  private static int m_RoleIndex;

  public static User UserData{get {return m_User;}set{m_User=value;}}
  public static ServerProperty ServerData { get { return m_Server; } set { m_Server = value; } }
  public static List<Role> RoleList { get { return m_RoleList; } set { m_RoleList = value; } }
  //当前登录角色
  public static Role RoleData 
  { 
    get { return m_RoleList[m_RoleIndex]; } 
    set 
    { 
      //将其它角色的委托去除
      for(int i=0;i<m_RoleList.Count;i++)
      {
        if(m_RoleList[i]==value)
        {
          m_RoleList[i].OnInfoChange += OnChange;
          m_RoleIndex = i;
        }
        else
        {
          m_RoleList[i].OnInfoChange -= OnChange;
        }
      }
      GameManager.Instance.OneSecondCallBack += RoleEnergyRecover;
    } 
  }

  
  public delegate void de();
  public static de OnRoleInfoChange;//角色信息改变的回调
  private static void OnChange()
  {
    if(OnRoleInfoChange!=null)
    {
      foreach(Action item in OnRoleInfoChange.GetInvocationList())
      {
        item();
      }
    }
  }

  private static void RoleEnergyRecover()
  {
    if (RoleData == null)
    {
      return;
    }
    if (RoleData.EnergyNextRecoverTimer > 0)
    {
      RoleData.EnergyNextRecoverTimer--;
    }
    if (RoleData.ToughenNextRecoverTimer > 0)
    {
      RoleData.ToughenNextRecoverTimer--;
    }
  }
}

