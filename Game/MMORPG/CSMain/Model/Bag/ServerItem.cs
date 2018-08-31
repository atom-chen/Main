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
namespace DB
{
    //物品的存储模型
    public partial class _DBItem
    {

        public _DBItem(Item item, int roleID)
        {
            this.Id = item.guid;
            this.tabID = item.tabID;
            this.count = item.count;
            this.roleID = roleID;
        }
    }


}
