using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DB
{
    //物品的存储模型
    public class _DBItem
    {
        public virtual int Id{get;set;}                  //物品id
        public virtual int tabID { get; set; }          //在表格中的id
        public virtual int count { get; set; }         //物品数量
        public virtual int roleID { set; get; }         //所属用户id
        public _DBItem()
        {

        }

        public _DBItem(Item item,int roleID)
        {
            this.Id = item.guid;
            this.tabID = item.tabID;
            this.count = item.count;
            this.roleID = roleID;
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
}
