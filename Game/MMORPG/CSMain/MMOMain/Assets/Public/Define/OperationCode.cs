﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/*
 * 消息码，表示发的是什么类型消息
 */ 
public enum OperationCode : byte
{
  Register,//注册消息
  EnterGame,//进入游戏
  RoleAdd,//添加角色
  StartGame,//开始游戏
}
