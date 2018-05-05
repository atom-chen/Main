
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

class User
{
  public int Guid=Define._INVALID_ID;
  public string UserName="";
  public string PassWord = "";
  public User()
  {

  }
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
class _DBUser
{
  public virtual int Guid { get; set; }
  public virtual string UserName { get; set; }
  public virtual string Password { get; set; }
}

//映射类
class UserMap : ClassMap<_DBUser>
{
  private string m_TableName = "User";
  public UserMap()
  {
    Id(x => x.Guid).Column("Guid");//设置GUID为key
    Map(x => x.UserName).Column("UserName");
    Map(x => x.Password).Column("Password");
    Table(m_TableName);
  }
}

