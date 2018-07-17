using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

//装备类
public partial class Equip : Item
{
    public int equipID;         //在Equip表格中的ID->tabid决定品质、主副属性
    public EquipAttr attr;
}

//装备属性
public class EquipAttr
{
    public int starLevel = 0;        //星级（强化）
}

