using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DB
{
    //物品的存储模型
    public partial class _DBItem
    {
        public virtual int Id{get;set;}                  //物品id
        public virtual int tabID { get; set; }          //在表格中的id
        public virtual int count { get; set; }         //物品数量
        public virtual int roleID { set; get; }         //所属用户id
        public _DBItem()
        {

        }
    }
}
