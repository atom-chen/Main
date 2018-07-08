using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

//装备类
public partial class Equip : Item
{
    public int equipID;//在Equip表格中的ID
    public EquipAttr attr;
}

//装备属性
public class EquipAttr
{
    public int starLevel = 1;//星级
    public int quality = 1;//品质
    public int damage = 0;//伤害
    public int hp = 0;//生命值
}

