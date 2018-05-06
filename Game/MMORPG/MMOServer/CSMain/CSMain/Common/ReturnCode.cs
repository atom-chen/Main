using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
/*
 *消息包的返回码
 */
public enum ReturnCode : short
{
  Success,
  Error,
  Fail,
  Exception,
  GetTeam,
  WartingTeam
}
