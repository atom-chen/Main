using Public.TableManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

class StrDictionary
{
    private static Dictionary<int, string> m_StrDic = new Dictionary<int, string>();
    static StrDictionary()
    {
        ReLoad();
    }

    public static void ReLoad()
    {
        try
        {
            m_StrDic.Clear();
            ReadTableTools table = new ReadTableTools("StrDictionary.txt");
            //将每个元组构造成Tab_Item，添加到表格
            while (!table.IsTupleEnd())
            {
                string data;

                //ID
                data = table.GetData();
                int id = Convert.ToInt32(data);

                //字符串
                data = table.GetNext();

                m_StrDic.Add(id, data);
                table.LineDown();
            }
        }
        catch(Exception)
        {
            
        }
    }

    public static string GetDicByID(int id,params object[] args)
    {
        string format = null;
        if(m_StrDic.TryGetValue(id,out format))
        {
            return string.Format(format, args);
        }
        return "";
    }
}

