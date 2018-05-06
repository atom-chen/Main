using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/*
 * 消息码，表示发的是什么类型消息
 */ 
public enum OperationCode : byte
{
  Login,
  GetServer,
  Register,
  Role,
  TaskDB,
  InventoryItemDB,
  SkillDB,
  Battle,
  Enemy,
  Boss
}
