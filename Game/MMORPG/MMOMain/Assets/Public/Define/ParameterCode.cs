using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/*
 * 返回的参数的返回码，表示返回的是什么类型参数
 */ 
public enum ParameterCode : byte
{
  Server,
  ServerList,
  User,
  RoleList,
  Role,
  ErrorInfo
}
