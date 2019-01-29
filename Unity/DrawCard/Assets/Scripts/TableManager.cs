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
    /// 还原现场，拿到所有已完成的人员名单
    /// </summary>
    /// <returns></returns>
    public static List<string> BuildEnvironment()
    {
        List<string> nameList = new List<string>();
        string[] drawLogs = File.ReadAllLines(GameDefine.RememberPath, Encoding.UTF8);               //读表2
        //遍历
        foreach(string log in drawLogs)
        {
            //按;切割
            string[] content = log.Split(' ', '\n', ';', '；', '	');
            //解析获奖类型
            if(content.Length<2)
            {
                continue;
            }
            switch(content[0])
            {
                case "3":
                    GameDefine.ThreeAlreadyDraw++;
                    break;
                case "2":
                    GameDefine.TwoAlreadyDraw++;
                    break;
                case "1":
                    GameDefine.OneAlreadyDraw++;
                    break;
                case "0":
                    GameDefine.BestAlreadyDraw++;
                    break;
            }
            //剩余的作为名字，加入到list
            for (int i = 1; i < content.Length; i++)
            {
                nameList.Add(content[i]);
            }
        }
        return nameList;
    }

    /// <summary>
    /// 读人员名单
    /// </summary>
    public static void InitPeopleList()
    {
        List<string> nameList = BuildEnvironment();
        string[] contents = File.ReadAllLines(GameDefine.TablePath, Encoding.UTF8);          //读表1
        for (int i = 0; i < contents.Length; i++)           //遍历每一行
        {
            string[] tuple = contents[i].Split('	', ';', '；', ' ');//按空格分割该元组
            if(tuple.Length<2)
            {
                continue;
            }
            string content = tuple[0] + tuple[1];
            if (nameList.Contains(content))
            {
                continue;
            }
            People people = new People();
            people.WorkID = tuple[0].Trim();
            people.Name = tuple[1].Trim();
            GameManager.AddItemToList(people);
        }
    }

    //读配置文件
    public static void ReadConfig()
    {
        string[] contents = File.ReadAllLines(GameDefine.ConfigPath, Encoding.UTF8);          //配置文件
        /*
         *ThreeCount;88;三等奖总人数
          ThreeEveryDraw;11;三等奖每次抽几个
          TwoCount;;二等奖总人数
          TwoEveryDraw;11;二等奖每次抽几个
          OneCount;30;一等奖总人数
          OneEveryDraw;10;一等奖每次抽几个
          BestCount;10;特等奖总人数
          BestEveryDraw;5;特等奖每次抽几个
         */
        for(int i =0;i<contents.Length;i++)
        {
            string[] content = contents[i].Split(';','；');
            //把数字读出来
            int num = 0;
            if(content.Length < 2)
            {
                continue;
            }
            if(int.TryParse(content[1],out num))
            {
                switch(i)
                {
                    case 0:
                        GameDefine.Total_Three = num;break;
                    case 1:
                        GameDefine.ThreeEveryDraw = num;
                        break;
                    case 2:
                        GameDefine.Total_Two = num;
                        break;
                    case 3:
                        GameDefine.TwoEveryDraw = num;
                        break;
                    case 4:
                        GameDefine.Total_One = num;
                        break;
                    case 5:
                        GameDefine.OneEveryDraw = num;
                        break;
                    case 6:
                        GameDefine.Total_Best = num;
                        break;
                    case 7:
                        GameDefine.BestEveryDraw = num;
                        break;
                }
            }
        }
    }

    /// <summary>
    /// 将中奖的人的名字写入表
    /// </summary>
    /// <param name="luckBoy">中奖的人名单</param>
    public static void RememberName(List<People> luckBoy,DRAW_TYPE type)
    {
        //写
        string writeText = "";
        switch(type)
        {
            case DRAW_TYPE.THREE:
                writeText += "3;";
                break;
            case DRAW_TYPE.TWO:
                writeText += "2;";
                break;
            case DRAW_TYPE.ONE:
                writeText += "1;";
                break;
            case DRAW_TYPE.BEST:
                writeText += "0;";
                break;
            case DRAW_TYPE.FREE:
                writeText += "4;";
                break;
        }

        for (int i = 0; i < luckBoy.Count; i++)
        {
            writeText += luckBoy[i].WorkID+luckBoy[i].Name + ";";
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

