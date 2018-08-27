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

