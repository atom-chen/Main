using Public.TableManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public class Tab_EquipAttachAttr
{
    public ATTR_TYPE attrType;
    public int CBase_Floor;
    public int CBase_Upper;
    public int CUp_Floor;
    public int CUp_Upper;

    public int BBase_Floor;
    public int BBase_Upper;
    public int BUp_Floor;
    public int BUp_Upper;

    public int ABase_Floor;
    public int ABase_Upper;
    public int AUp_Floor;
    public int AUp_Upper;

    public int SBase_Floor;
    public int SBase_Upper;
    public int SUp_Floor;
    public int SUp_Upper;

    public int SSBase_Floor;
    public int SSBase_Upper;
    public int SSUp_Floor;
    public int SSUp_Upper;

    public int SSSBase_Floor;
    public int SSSBase_Upper;
    public int SSSUp_Floor;
    public int SSSUp_Upper;
}

//根据珍惜程度获取
public class EquipAttachAttr_Rare
{
    public int baseVal_Floor;
    public int baseVal_Upper;
    public int upVal_Floor;
    public int upval_Upper;
}

public class TabEquipAttachManager
{
    private static Dictionary<ATTR_TYPE, Tab_EquipAttachAttr> m_TabEquipAttachAttrDic = new Dictionary<ATTR_TYPE, Tab_EquipAttachAttr>();
    static TabEquipAttachManager()
    {
        ReLoad();
    }

    public static void ReLoad()
    {
        try
        {


            m_TabEquipAttachAttrDic.Clear();
            ReadTableTools table = new ReadTableTools("EquipAttachAttr.txt");
            //将每个元组构造成Tab_Item，添加到表格
            while (!table.IsTupleEnd())
            {
                string data;
                Tab_EquipAttachAttr attr = new Tab_EquipAttachAttr();

                //属性类型
                data = table.GetData();
                attr.attrType = (ATTR_TYPE)Convert.ToInt32(data);

                //C
                data = table.GetNext();
                attr.CBase_Floor = Convert.ToInt32(data);
                data = table.GetNext();
                attr.CBase_Upper = Convert.ToInt32(data);
                data = table.GetNext();
                attr.CUp_Floor = Convert.ToInt32(data);
                data = table.GetNext();
                attr.CUp_Upper = Convert.ToInt32(data);

                //B
                data = table.GetNext();
                attr.BBase_Floor = Convert.ToInt32(data);
                data = table.GetNext();
                attr.BBase_Upper = Convert.ToInt32(data);
                data = table.GetNext();
                attr.BUp_Floor = Convert.ToInt32(data);
                data = table.GetNext();
                attr.BUp_Upper = Convert.ToInt32(data);

                //A
                data = table.GetNext();
                attr.ABase_Floor = Convert.ToInt32(data);
                data = table.GetNext();
                attr.ABase_Upper = Convert.ToInt32(data);
                data = table.GetNext();
                attr.AUp_Floor = Convert.ToInt32(data);
                data = table.GetNext();
                attr.AUp_Upper = Convert.ToInt32(data);

                //S
                data = table.GetNext();
                attr.SBase_Floor = Convert.ToInt32(data);
                data = table.GetNext();
                attr.SBase_Upper = Convert.ToInt32(data);
                data = table.GetNext();
                attr.SUp_Floor = Convert.ToInt32(data);
                data = table.GetNext();
                attr.SUp_Upper = Convert.ToInt32(data);

                //SS
                data = table.GetNext();
                attr.SSBase_Floor = Convert.ToInt32(data);
                data = table.GetNext();
                attr.SSBase_Upper = Convert.ToInt32(data);
                data = table.GetNext();
                attr.SSUp_Floor = Convert.ToInt32(data);
                data = table.GetNext();
                attr.SSUp_Upper = Convert.ToInt32(data);

                //SS
                data = table.GetNext();
                attr.SSSBase_Floor = Convert.ToInt32(data);
                data = table.GetNext();
                attr.SSSBase_Upper = Convert.ToInt32(data);
                data = table.GetNext();
                attr.SSSUp_Floor = Convert.ToInt32(data);
                data = table.GetNext();
                attr.SSSUp_Upper = Convert.ToInt32(data);


                m_TabEquipAttachAttrDic.Add(attr.attrType, attr);
                table.LineDown();
            }
        }
        catch(Exception)
        {
            
        }
    }

    //根据类型获取
    public static Tab_EquipAttachAttr GetEquipAttachByAttrType(ATTR_TYPE type)
    {
        Tab_EquipAttachAttr ret = null;
        if(m_TabEquipAttachAttrDic.TryGetValue(type,out ret))
        {
            return ret;
        }
        return null;
    }

    public static EquipAttachAttr_Rare GetEquipAttachByAttrType(ATTR_TYPE type,EQUIP_RARE rare)
    {
        Tab_EquipAttachAttr attachAttrVal = null;
        if (m_TabEquipAttachAttrDic.TryGetValue(type, out attachAttrVal))
        {
            EquipAttachAttr_Rare ret = new EquipAttachAttr_Rare();
            switch(rare)
            {
                case EQUIP_RARE.C:
                    ret.baseVal_Floor = attachAttrVal.CBase_Floor;
                    ret.baseVal_Upper = attachAttrVal.CBase_Upper;
                    ret.upVal_Floor = attachAttrVal.CUp_Floor;
                    ret.upval_Upper = attachAttrVal.CUp_Upper;
                    break;
                case EQUIP_RARE.B:
                    ret.baseVal_Floor = attachAttrVal.BBase_Floor;
                    ret.baseVal_Upper = attachAttrVal.BBase_Upper;
                    ret.upVal_Floor = attachAttrVal.BUp_Floor;
                    ret.upval_Upper = attachAttrVal.BUp_Upper;
                    break;
                case EQUIP_RARE.A:
                    ret.baseVal_Floor = attachAttrVal.ABase_Floor;
                    ret.baseVal_Upper = attachAttrVal.ABase_Upper;
                    ret.upVal_Floor = attachAttrVal.AUp_Floor;
                    ret.upval_Upper = attachAttrVal.AUp_Upper;
                    break;
                case EQUIP_RARE.S:
                    ret.baseVal_Floor = attachAttrVal.SBase_Floor;
                    ret.baseVal_Upper = attachAttrVal.SBase_Upper;
                    ret.upVal_Floor = attachAttrVal.SUp_Floor;
                    ret.upval_Upper = attachAttrVal.SUp_Upper;
                    break;
                case EQUIP_RARE.SS:
                    ret.baseVal_Floor = attachAttrVal.SSBase_Floor;
                    ret.baseVal_Upper = attachAttrVal.SSBase_Upper;
                    ret.upVal_Floor = attachAttrVal.SSUp_Floor;
                    ret.upval_Upper = attachAttrVal.SSUp_Upper;
                    break;
                case EQUIP_RARE.SSS:
                    ret.baseVal_Floor = attachAttrVal.SSSBase_Floor;
                    ret.baseVal_Upper = attachAttrVal.SSSBase_Upper;
                    ret.upVal_Floor = attachAttrVal.SSSUp_Floor;
                    ret.upval_Upper = attachAttrVal.SSSUp_Upper;
                    break;
            }
            return ret;
        }
        return null;
    }
}

