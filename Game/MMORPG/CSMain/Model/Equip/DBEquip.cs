using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DB
{
    //装备存储类
    public partial class _DBEquip
    {
        public virtual int guid { get; set; }
        public virtual int equipId { get; set; }
        public virtual int star { get; set; }
        public virtual int roleId { get; set; }
        public _DBEquip()
        {
            
        }
    }
}


