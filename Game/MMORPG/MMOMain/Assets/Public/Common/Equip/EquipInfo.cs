using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

//身上穿的装备
public class EquipInfo
{
    private Equip[] mEquipArray;
    public Equip Weapon { get { return mEquipArray[(int)EQUIP_TYPE.WEAPON]; } }
    public Equip Shuko { get { return mEquipArray[(int)EQUIP_TYPE.SHUKO]; } }
    public Equip Cloth { get { return mEquipArray[(int)EQUIP_TYPE.CLOTH]; } }
    public Equip Helm { get { return mEquipArray[(int)EQUIP_TYPE.HELM]; } }
    public Equip Trousers { get { return mEquipArray[(int)EQUIP_TYPE.TROUSERS]; } }
    public Equip Shoes { get { return mEquipArray[(int)EQUIP_TYPE.SHOES]; } }
    public Equip Ring { get { return mEquipArray[(int)EQUIP_TYPE.RING]; } }
    public Equip Wing { get { return mEquipArray[(int)EQUIP_TYPE.WING]; } }

    public EquipInfo()
    {
        mEquipArray = new Equip[(int)EQUIP_TYPE.MAX];
    }

    //穿上装备
    public Equip FuncEquip(Equip equip)
    {
        Tab_Equip tabEquip = TabEquipManager.GetEquipByID(equip.equipID);
        if(tabEquip!=null)
        {
            int index = (int)tabEquip.area;          //穿戴位置
            if(index<mEquipArray.Length)
            {
                Equip old = mEquipArray[index];
                mEquipArray[index] = equip;
                return old;
            }
        }
        return null;
    }

    //卸下装备
    public Equip UnEquip(int index)
    {
        if(index<mEquipArray.Length)
        {
            Equip ret = mEquipArray[index];
            mEquipArray[index] = null;
            return ret;
        }
        return null;
    }

    //获取装备信息
    public Equip GetEquip(int index)
    {
        if(index<mEquipArray.Length)
        {
            return mEquipArray[index];
        }
        return null;
    }

    public Equip[] GetArray()
    {
        return mEquipArray;
    }
}

