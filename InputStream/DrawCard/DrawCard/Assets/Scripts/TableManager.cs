using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
/// <summary>
/// 表管理器
/// </summary>
class TableManager
{
    /// <summary>
    /// 读表1
    /// </summary>
    public static void InitPeopleList()
    {
        string[] contents = File.ReadAllLines(GameDefine.TablePath, Encoding.UTF8);          //读表1
        string names = File.ReadAllText(GameDefine.RememberPath, Encoding.UTF8);               //读表2
        List<string> nameList = new List<string>(names.Split(' '));
        for (int i = 0; i < contents.Length; i++)           //遍历每一行
        {
            string[] tuple = contents[i].Split('	');//按空格分割该元组
            if(nameList.Contains(tuple[1]))
            {
                continue;
            }
            Console.WriteLine(tuple.Length);
            People people = new People();
            people.WorkID = tuple[0].Trim();
            people.Name = tuple[1].Trim();
            people.ProjectGroup = tuple[2].Trim();
            people.Department = tuple[3].Trim();
            GameLogic.Instance().AddItemToList(people);
        }
    }
    /// <summary>
    /// 将中奖的人的名字写入表
    /// </summary>
    /// <param name="luckBoy">中奖的人名单</param>
    public static void RememberName(List<People> luckBoy)
    {
        //写
        string writeText = "";
        for (int i = 0; i < luckBoy.Count; i++)
        {

            writeText += luckBoy[i].Name + " ";

        }
        //打开文件流
        FileStream fs;
        StreamWriter sw;
        fs = new FileStream(GameDefine.RememberPath, FileMode.Append);
        sw = new StreamWriter(fs);
        sw.WriteLine(writeText);
        sw.Close();
        fs.Close();
    }

}

