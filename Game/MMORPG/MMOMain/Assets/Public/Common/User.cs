using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
/*
 * User模型:应用层
 */

public partial class User
{
    public int Guid = Define._INVALID_ID;
    public string UserName = "";
    public string PassWord = "";

    private  List<Role> m_RoleList = new List<Role>();
    private  int m_RoleIndex;

    //用户下的角色列表
    public List<Role> RoleList { get { return m_RoleList; } set { m_RoleList = value; } }  

    //获取当前登陆角色
    public Role RoleData    
    {
        get { if (m_RoleIndex >= m_RoleList.Count) return null; return m_RoleList[m_RoleIndex]; }
        set
        {
            for (int i = 0; i < m_RoleList.Count; i++)
            {
                if (m_RoleList[i].id == value.id)
                {
                    m_RoleList[i] = value;
                    m_RoleIndex = i;
                }
            }
        }
    }

    public User()
    {

    }

}



