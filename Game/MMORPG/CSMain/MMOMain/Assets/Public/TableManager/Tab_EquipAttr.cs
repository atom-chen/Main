using Public.TableManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public class Tab_EquipAttr
{
   public int id;
   public ATTR_TYPE type;    //增加属性的类型
   public float[] value = new float[Define._MAX_STRENGTHEN_ + 1];    //最大强化级别
}

public class TabEquipAttrManager
{
    private static Dictionary<int, Tab_EquipAttr> m_TabEquipAttrDic = new Dictionary<int, Tab_EquipAttr>();
    static TabEquipAttrManager()
    {
        ReLoad();
    }

    public static void ReLoad()
    {
        try
        {
            m_TabEquipAttrDic.Clear();
            ReadTableTools table = new ReadTableTools("EquipAttr.txt");
            //将每个元组构造成Tab_Item，添加到表格
            while (!table.IsTupleEnd())
            {
                string data;
                Tab_EquipAttr attr = new Tab_EquipAttr();

                //ID
                data = table.GetData();
                attr.id = Convert.ToInt32(data);

                //装备类型
                data = table.GetNext();
                attr.type = (ATTR_TYPE)Convert.ToInt32(data);

                //属性值
                for (int i = 0; i <= Define._MAX_STRENGTHEN_; i++)
                {
                    data = table.GetNext();
                    attr.value[i] = Convert.ToInt32(data);
                }

                m_TabEquipAttrDic.Add(attr.id, attr);
                table.LineDown();
            }
        }
        catch(Exception)
        {

        }
    }


    public static Tab_EquipAttr GetEquipByID(int equipID)
    {
        Tab_EquipAttr ret;
        if (m_TabEquipAttrDic.TryGetValue(equipID, out ret))
        {
            return ret;
        }
        return null;
    }
}
