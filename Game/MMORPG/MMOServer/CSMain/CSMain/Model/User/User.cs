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
 * User模型
 */ 

public partial class User
{
  public User(_DBUser dbUser)
  {
    Guid = dbUser.Guid;
    UserName = dbUser.UserName;
    PassWord = dbUser.Password;
  }
  public void CopyForm(_DBUser dbUser)
  {
    Guid = dbUser.Guid;
    UserName = dbUser.UserName;
    PassWord = dbUser.Password;
  }
}



