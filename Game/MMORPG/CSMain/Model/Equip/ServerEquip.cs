using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public partial class Equip
{
    public Equip(DB._DBEquip db)
    {
        this.guid = db.guid;
        this.equipID = db.equipId;
        this.starLevel = db.star;
    }
}

namespace DB
{
    //装备存储类
    public partial class _DBEquip
    {
        public _DBEquip(Equip equip, int roleId)
        {
            this.guid = equip.guid;
            this.equipId = equip.equipID;
            this.star = equip.starLevel;
            this.roleId = roleId;
        }
    }


}