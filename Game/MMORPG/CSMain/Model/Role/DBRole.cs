using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DB
{
    public partial class _DBRole
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
        
        //装备
        public virtual int Helm { get; set; }
        public virtual int nCloth { get; set; }
        public virtual int Weapon { get; set; }
        public virtual int Shoes { get; set; }
        public virtual int NeckLace { get; set; }
        public virtual int BraceLet { get; set; }
        public virtual int Ring { get; set; }
        public virtual int Wing { get; set; }

        public _DBRole()
        {

        }


    }
}