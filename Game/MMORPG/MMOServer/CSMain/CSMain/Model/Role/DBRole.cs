using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DB
{
    public class _DBRole
    {
        public virtual int ID { get; set; }
        public virtual string Name { get; set; }
        public virtual uint Level { get; set; }
        public virtual bool Sex { get; set; }
        public virtual int UserID { get; set; }
        public virtual string HeadIcon { get; set; }
        public virtual int Exp { get; set; }
        public virtual int Coin { get; set; }
        public virtual int YuanBao { get; set; }
        public virtual int Energy { get; set; }
        public virtual int Toughen { get; set; }
        public virtual string LastDownLine { get; set; }

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
            HeadIcon = role.HeadIcon;
            Exp = role.Exp;
            Coin = role.Coin;
            YuanBao = role.YuanBao;
            Energy = role.Energy;
            Toughen = role.Toughen;
        }
    }

    public class _DBRoleMap : ClassMap<_DBRole>
    {
        public _DBRoleMap()
        {
            LazyLoad();
            Id(x => x.ID).Column("Id");
            Map(x => x.Name).Column("name");
            Map(x => x.Level).Column("Level");
            Map(x => x.Sex).Column("Sex");
            Map(x => x.UserID).Column("UserID");
            Map(x => x.HeadIcon).Column("HeadIcon");
            Map(x => x.Exp).Column("Exp");
            Map(x => x.Coin).Column("coin");
            Map(x => x.YuanBao).Column("YuanBao");
            Map(x => x.Energy).Column("Energy");
            Map(x => x.Toughen).Column("Toughen");
            Map(x => x.LastDownLine).Column("LastDownLine");
            Table("role");
        }
    }
}