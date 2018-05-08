﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/*
 * 消息码，表示发的是什么类型消息
 */ 
public enum OperationCode : byte
{
  Login,//登录消息
  GetServer,//获取服务器列表消息
  Register,//注册消息
  Role,
  TaskDB,
  InventoryItemDB,
  SkillDB,
  Battle,
  Enemy,
  Boss
}
