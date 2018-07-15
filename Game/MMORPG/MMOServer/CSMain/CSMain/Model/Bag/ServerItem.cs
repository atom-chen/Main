using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public partial class Item
{
    public Item(DB._DBItem dbItem)
    {
        this.guid = dbItem.Id;
        this.count = dbItem.count;
        this.tabID = dbItem.tabID;
    }
}

