using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


  public class Role
  {
    public int ID;//ID
    public string Name;//名称
    public uint Level;//等级
    public bool Sex;//性别
    public int UserID;//所属用户
    public string HeadIcon;//头像
    public int Exp;//经验
    public int Coin;//金币
    public int Energy;//体力
    public int Toughen;//历练数
    public Role()
    {

    }
    public Role(DB._DBRole db)
    {
      this.ID = db.ID;
      this.Name = db.Name;
      this.Level = db.Level;
      this.Sex = db.Sex;
      this.UserID = db.UserID;
    }
  }

  namespace DB
  {
    public class _DBRole
    {
      public virtual int ID { get; set; }
      public virtual string Name { get; set; }
      public virtual uint Level { get; set; }
      public virtual bool Sex { get; set; }
      public virtual int UserID { get; set; }
      

      public _DBRole()
      {

      }

      public _DBRole(Role role)
      {
        ID = role.ID;
        Name = role.Name;
        Level = role.Level;
        Sex = role.Sex;
        UserID = role.UserID;
      }
    }

   public  class _DBRoleMap:ClassMap<_DBRole>
    {
      public _DBRoleMap()
      {
        LazyLoad();
        Id(x => x.ID).Column("Id");
        Map(x => x.Name).Column("name");
        Map(x => x.Level).Column("Level");
        Map(x => x.Sex).Column("Sex");
        Map(x => x.UserID).Column("UserID");
        Table("role");
      }
    }
  }