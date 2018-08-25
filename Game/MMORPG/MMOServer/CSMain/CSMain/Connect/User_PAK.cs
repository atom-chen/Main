using Photon.SocketServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public partial class User
{
    private UserConnect m_Connect;
    public UserConnect _Connect { set { m_Connect = value; } }

    public void SendPak(GC_PAK_BASE pak, Photon.SocketServer.SendParameters para = new SendParameters())
    {
        m_Connect.SendOperationResponse(pak._Response, para);
    }


    public void HandlePacket(CG_ENTER_GAME_PAK package)
    {
        //user信息
        User user = package._User;
        int guid = -1;
        //成功
        if ((guid = UserManager.Login(user)) != Define._INVALID_ID)
        {
            user.Guid = guid;
            CopyFrom(user);
            //拿到该玩家所有角色信息
            List<Role> roleList = RoleManager.GetUserAllRole(this.Guid);
            GC_ENTER_GAME_RET_PAK pak = new GC_ENTER_GAME_RET_PAK();
            pak._ReturnCode = ReturnCode.Success;
            pak._User = user;
            pak.RoleList = roleList;
            SendPak(pak);
        }
        //密码错误
        else
        {
            GC_ENTER_GAME_RET_PAK pak = new GC_ENTER_GAME_RET_PAK();
            pak._ReturnCode = ReturnCode.Fail;
            pak.ErrorInfo = "密码错误";
        }     
    }
    public void HandlePacket(CG_REGISTER_PAK package)
    {
        User newUser = package._User;
        if (newUser != null)
        {
            bool isSuccess = UserManager.Register(newUser);
            //成功
            if (isSuccess)
            {
                this.CopyFrom(newUser);
                GC_REGISTER_USER_RET_PAK pak = new GC_REGISTER_USER_RET_PAK();
                pak._ReturnCode = ReturnCode.Success;
                pak._User = newUser;
                SendPak(pak);
            }
            else
            {
                GC_REGISTER_USER_RET_PAK pak = new GC_REGISTER_USER_RET_PAK();
                pak._ReturnCode = ReturnCode.Fail;
                pak.ErrorInfo = "用户名重复";
                SendPak(pak);
            }
        }
    }
    public void HandlePacket(CG_ADD_ROLE_PAK package)
    {
        Role pakRole = package._Role;
        if (pakRole != null)
        {
            Role newRole = new Role(pakRole.name, pakRole.sex, this.Guid);
            if (RoleManager.CreateRole(newRole))
            {
                //创建成功
                GC_ROLE_ADD_RET_PAK pak = new GC_ROLE_ADD_RET_PAK();
                pak._ReturnCode = ReturnCode.Success;
                pak._Role = newRole;
                SendPak(pak);
            }
            else
            {
                GC_ROLE_ADD_RET_PAK pak = new GC_ROLE_ADD_RET_PAK();
                pak._ReturnCode = ReturnCode.Fail;
                pak.ErrorInfo = "此名称已存在";
                SendPak(pak);
            }
        }
    }
    public void HandlePacket(CG_START_GAME_PAK package)
    {
        Role msgRole = package._Role;
        if (msgRole != null)
        {
            Role newRole = RoleManager.RoleOnline(msgRole);             //角色上线
            if (newRole != null)
            {
                this.RoleData = newRole;
                if(RoleData!=null)
                {
                    GC_START_GAME_RET_PAK pak = new GC_START_GAME_RET_PAK();
                    pak._ReturnCode = ReturnCode.Success;
                    pak._Role = RoleData;
                    SendPak(pak);
                }
            }
        }
    }
}

