using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public partial class PlayData
{
    public static void ReceivePacket(GC_REGISTER_USER_RET_PAK pak)
    {
        if(pak.Success)
        {
            //返回到选服
            Tips.ShowTip("注册成功，已为您登录");
            //if (StartMenu.Instance != null)
            //{
            //    User user = ParaTools.GetParameter<User>(response.Parameters, ParameterCode.User);
            //    StartMenu.Instance.LoginSuccessed(user);
            //}
        }
        else
        {
            Tips.ShowTip(pak.ErrorInfo);
        }
    }

    public static void ReceivePacket(GC_ENTER_GAME_RET_PAK pak)
    {
        //解析角色信息
        List<Role> roleList = pak.RoleList;
        //LaunchSceneLogic.Instance.SetRoleList(roleList);
        Tips.ShowTip("登录成功");

        //User信息
        //if (StartMenu.Instance != null)
        //{
        //    User user = ParaTools.GetParameter<User>(response.Parameters, ParameterCode.User);
        //    StartMenu.Instance.LoginSuccessed(user);
        //}
    }

    public static void ReceivePacket(GC_ROLE_ADD_RET_PAK pak)
    {
        ////添加角色成功
        //LaunchSceneLogic.Instance.AddRoleSuccess(pak._Role);
        //Tips.ShowTip("添加角色成功");
    }
    public static void ReceivePacket(GC_START_GAME_RET_PAK pak)
    {
        //解析角色信息
        Role role = pak._Role;
        PlayData.RoleData = role;
        //切换场景
        UnityEngine.SceneManagement.SceneManager.LoadScene(1);
    }
}

