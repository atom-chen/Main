using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DB
{
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

            Map(x => x.Helm).Column("Helm");
            Map(x => x.nCloth).Column("Cloth");
            Map(x => x.Weapon).Column("Weapon");
            Map(x => x.Shoes).Column("Shoes");
            Map(x => x.NeckLace).Column("NeckLace");
            Map(x => x.BraceLet).Column("BraceLet");
            Map(x => x.Ring).Column("Ring");
            Map(x => x.Wing).Column("Wing");
            Table("role");
        }
    }

    public class _DBItemMap : ClassMap<_DBItem>
    {
        public _DBItemMap()
        {
            LazyLoad();
            Id(x => x.Id).Column("Id");
            Map(x => x.tabID).Column("tabID");
            Map(x => x.count).Column("count");
            Map(x => x.roleID).Column("roleID");
            Table("item");
        }
    }

    public class _DBEquipMap : ClassMap<_DBEquip>
    {
        public _DBEquipMap()
        {
            LazyLoad();
            Id(x => x.guid).Column("Id");
            Map(x => x.equipId).Column("tabId");
            Map(x => x.star).Column("star");
            Map(x => x.roleId).Column("roleId");
            Table("equip");
        }
    }
}
