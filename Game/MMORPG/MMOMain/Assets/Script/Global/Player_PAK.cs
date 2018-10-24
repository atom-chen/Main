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
            Tips.ShowTip("注册成功");
            if (LaunchSceneLogic.Instance != null)
            {
                LaunchSceneLogic.Instance.HandlePackage(pak);
            }
        }
        else
        {
            Tips.ShowTip(pak.ErrorInfo);
        }
    }

    public static void ReceivePacket(GC_ENTER_GAME_RET_PAK pak)
    {
        GameManager.PlayerData = new PlayData();
        GameManager.PlayerData.RoleList = pak._User.RoleList;
        Tips.ShowTip("登录成功");

        //User信息
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
        GameManager.PlayerData.RoleData = role;
        //切换场景
        SceneMgr.LoadScene(SCENE_CODE.MAIN);
    }
}

