using Public.TableManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public class Tab_Item
{
    public int id;//ID
    public string name;//NAME
    public string icon;//Icon
    public int classType;//所属类型
    public int infoType;//二级分类类型
    public int level = 1;//使用等级
    public int maxCount = 1;//最大数量
    public int price;//出售价格
    public string desc;//描述
}

public class TabItem_Manager
{
    private static Dictionary<int, Tab_Item> m_TabItemDic = new Dictionary<int, Tab_Item>();
    private TabItem_Manager()
    {

    }
    static TabItem_Manager()
    {
        ReLoad();
    }
    
    public static void ReLoad()
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
            item.icon = data;

            //Class Type
            data = table.GetNext();
            item.classType = Convert.ToInt32(data);

            //sub Type
            data = table.GetNext();
            item.infoType = Convert.ToInt32(data);

            //level
            data = table.GetNext();
            item.level = Convert.ToInt32(data);

            //max count
            data = table.GetNext();
            item.maxCount = Convert.ToInt32(data);

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
    public static Tab_Item GetItemByID(int id)
    {
        Tab_Item item;
        m_TabItemDic.TryGetValue(id, out item);
        return item;
    }
}
