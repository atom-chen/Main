
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

public class User
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


namespace DB
{
  public class _DBUser
  {
    public virtual int Guid { get; set; }
    public virtual string UserName { get; set; }
    public virtual string Password { get; set; }
    public _DBUser()
    {

    }
    public _DBUser(User other)
    {
      this.Guid = other.Guid;
      this.UserName = other.UserName;
      this.Password = other.PassWord;
    }
  }

  //映射类
  public class UserMap : ClassMap<_DBUser>
  {
    private string m_TableName = "User";
    public UserMap()
    {
      LazyLoad();
      Id(x => x.Guid).Column("Guid");//设置GUID为key
      Map(x => x.UserName).Column("UserName");
      Map(x => x.Password).Column("Password");
      Table(m_TableName);
    }
  }
}


