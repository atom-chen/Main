using DB;
using FluentNHibernate.Mapping;
using Photon.SocketServer;
using PhotonHostRuntimeInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
/*
 * 服务器User模型
 */

public partial class User
{
    public void BuildFromDB(DM_UserConnectMsg msg)
    {
        this.Guid = msg.User.Guid;
        this.PassWord = msg.User.Password;
        this.UserName = msg.User.UserName;
        for(int i = 0;i<msg.RoleList.Count ;i++)
        {
            Role role = new Role(msg.RoleList[i],msg.ItemList,msg.EquipList);
            this.RoleList.Add(role);
        }
    }
    public void BuildFromDB(DM_UserMsg msg)
    {
        this.Guid = msg.User.Guid;
        this.PassWord = msg.User.Password;
        this.UserName = msg.User.UserName;
        for (int i = 0; i < msg.RoleList.Count; i++)
        {
            Role role = new Role(msg.RoleList[i], msg.ItemList, msg.EquipList);
            this.RoleList.Add(role);
        }
    }
}



namespace DB
{
    public partial class _DBUser
    {
        public _DBUser(User other)
        {
            this.Guid = other.Guid;
            this.UserName = other.UserName;
            this.Password = other.PassWord;
        }
    }
}