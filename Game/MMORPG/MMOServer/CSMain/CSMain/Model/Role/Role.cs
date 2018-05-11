using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


  public class Role
  {
    public int ID;
    public string Name;
    public uint Level;
    public bool Sex;
    public int UserID;
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