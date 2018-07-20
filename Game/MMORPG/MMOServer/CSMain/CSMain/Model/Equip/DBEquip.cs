using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DB
{
    //装备存储类
    public class _DBEquip
    {
        public virtual int guid { get; set; }
        public virtual int equipId { get; set; }
        public virtual int star { get; set; }
        public virtual int roleId { get; set; }
        public _DBEquip()
        {
            
        }
        public _DBEquip(Equip equip,int roleId)
        {
            this.guid = equip.guid;
            this.equipId = equip.equipID;
            this.star = equip.starLevel;
            this.roleId = roleId;
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


