using Public.TableManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class Tab_Equip
{
    public int id;
    public int type;
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

            //Type
            data = table.GetNext();
            equip.type = Convert.ToInt32(data);

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

