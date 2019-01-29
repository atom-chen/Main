using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
/// <summary>
/// 玩家数据
/// </summary>
/// 

public partial class PlayData
{
    private User m_User;
    private Tab_Server m_Server;

    public User UserData { get { return m_User; } set { m_User = value; } } //当前用户信息
    public Tab_Server ServerData { get { return m_Server; } set { m_Server = value; } } //当前登录服务器
    public List<Role> RoleList { get { return m_User.RoleList; } set { m_User.RoleList = value; } }  //用户下的角色列表

    //当前登录角色
    public Role RoleData
    {
        get { return m_User.RoleData; }
        set
        {
            //将其它角色的委托去除
            for (int i = 0; i < m_User.RoleList.Count; i++)
            {
                RoleList[i].OnInfoChange -= OnChange;
            }
            RoleData = value;
            RoleData.OnInfoChange += OnChange;
        }
    }

    public delegate void RoleEvent();
    public event RoleEvent OnRoleInfoChange;//角色信息改变的回调
    private void OnChange()
    {
        if (OnRoleInfoChange != null)
        {
            OnRoleInfoChange();
        }
    }

    /// <summary>
    /// 客户端层面 体力恢复计时
    /// </summary>
    private void RoleEnergyRecover()
    {
        if (RoleData == null)
        {
            return;
        }
        if (RoleData.energyNextRecoverTimer > 0)
        {
            RoleData.energyNextRecoverTimer--;
        }
        if (RoleData.toughenNextRecoverTimer > 0)
        {
            RoleData.toughenNextRecoverTimer--;
        }
    }
}

