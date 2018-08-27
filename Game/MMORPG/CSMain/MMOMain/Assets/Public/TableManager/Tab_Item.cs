using Public.TableManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public class Tab_Item
{
    public int id;//ID
    public string name;//NAME
    public string atlas;//图集名称
    public string icon;//Icon
    public ITEM_FIRST classType;//所属类型
    public int price;//出售价格
    public string desc;//描述
}

public class TabItemManager
{
    private static Dictionary<int, Tab_Item> m_TabItemDic = new Dictionary<int, Tab_Item>();
    private TabItemManager()
    {

    }
    static TabItemManager()
    {
        ReLoad();
    }
    
    public static void ReLoad()
    {
        try
        {
            m_TabItemDic.Clear();
            ReadTableTools table = new ReadTableTools("Item.txt");
            //将每个元组构造成Tab_Item，添加到表格
            while (!table.IsTupleEnd())
            {
                string data;
                Tab_Item item = new Tab_Item();

                //ID
                data = table.GetData();
                item.id = Convert.ToInt32(data);

                //Name
                data = table.GetNext();
                item.name = data;

                //Icon
                data = table.GetNext();
                item.atlas = data;

                //Icon
                data = table.GetNext();
                item.icon = data;

                //Class Type
                data = table.GetNext();
                item.classType = (ITEM_FIRST)Convert.ToInt32(data);


                //Price
                data = table.GetNext();
                item.price = Convert.ToInt32(data);

                //desc
                data = table.GetNext();
                item.desc = data;

                m_TabItemDic.Add(item.id, item);
                table.LineDown();
            }
        }
        catch(Exception)
        {

        }

    }
    public static Tab_Item GetItemByID(int id)
    {
        Tab_Item item;
        m_TabItemDic.TryGetValue(id, out item);
        return item;
    }
}
