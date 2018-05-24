using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public enum OperationCode : byte
{
  Login,//登录消息
  GetServer,//获取服务器列表消息
  Register,//注册消息
  EnterGame,//进入游戏
  RoleAdd,//添加角色
  StartGame,//开始游戏
}
