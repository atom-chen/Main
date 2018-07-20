using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

//身上穿的装备
public class EquipInfo
{
    private Equip[] mEquipArray;
    public EquipInfo()
    {
        if(Config.EquipMaxCount>0)
        {
            mEquipArray = new Equip[Config.EquipMaxCount];
        }
        else
        {
            mEquipArray = new Equip[1];
        }
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
}

