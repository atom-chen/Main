using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
public struct EquipAttachAttr
{
    public ATTR_TYPE _Type;
    public int val;
}

//装备类
public partial class Equip
{
    public int guid;
    public int equipID;         //在Equip表格中的ID->tabid决定品质、主副属性
    public List<EquipAttachAttr> attachAttribute=new List<EquipAttachAttr>();     //附加属性：根据品质增幅不同
    public int AttachAttrCount { get { return attachAttribute.Count; } }     //附加属性条数
    public int starLevel = 0;   //星级（强化）

    public Equip()
    {

    }

    public Tab_Equip GetTabEquip()
    {
        return TabEquipManager.GetEquipByID(equipID);
    }

    public Tab_Item GetTabItem()
    {
        Tab_Equip tabEquip = GetTabEquip();
        if(tabEquip!=null)
        {
            return TabItemManager.GetItemByID(tabEquip.itemId);
        }
        return null;
    }


}



