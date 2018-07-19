using Public.TableManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class Tab_Equip
{
    public int id;
    public EQUIP_TYPE area;  //装备部位
    public EQUIP_RARE rare;  //装备品质
    public int mainAttrid;   //主属性iD
    public int assistAttrid; //副属性id
    public int needLevel;    //装备等级
    public int itemId;       //在Item表的ID
}

public class TabEquipManager
{
    private static Dictionary<int, Tab_Equip> m_TabEquipDic = new Dictionary<int, Tab_Equip>();
    static TabEquipManager()
    {
        ReLoad();
    }

    public static void ReLoad()
    {
        m_TabEquipDic.Clear();
        ReadTableTools table = new ReadTableTools("Equip.txt");
        //将每个元组构造成Tab_Item，添加到表格
        while (!table.IsTupleEnd())
        {
            string data;
            Tab_Equip equip = new Tab_Equip();

            //ID
            data = table.GetData();
            equip.id = Convert.ToInt32(data);

            //装备位置
            data = table.GetNext();
            equip.area =(EQUIP_TYPE) Convert.ToInt32(data);

            //装备品质
            data = table.GetNext();
            equip.rare = (EQUIP_RARE)Convert.ToInt32(data);

            //主属性ID
            data = table.GetNext();
            equip.mainAttrid = Convert.ToInt32(data);

            //副属性ID
            data = table.GetNext();
            equip.assistAttrid = Convert.ToInt32(data);

            //装备等级
            data = table.GetNext();
            equip.needLevel = Convert.ToInt32(data);

            //ItemID
            data = table.GetNext();
            equip.itemId = Convert.ToInt32(data);

            m_TabEquipDic.Add(equip.id, equip);
            table.LineDown();
        }
    }

    
    public static Tab_Equip GetEquipByID(int equipID)
    {
        Tab_Equip ret;
        if(m_TabEquipDic.TryGetValue(equipID,out ret))
        {
            return ret;
        }
        return null;
    }
}

