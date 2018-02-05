using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using UnityEngine;
/// <summary>
/// 表管理器
/// </summary>
class TableManager:MonoBehaviour
{
    public static void InitPeopleList()
    {
		string[] contents = File.ReadAllLines (GameDefine.TablePath,Encoding.Default);
        Console.WriteLine("contents:"+contents.Length);
        for(int i=0;i<contents.Length;i++)           //遍历每一行
        {
			string[] tuple = contents[i].Split('	');//按空格分割该元组
            Console.WriteLine(tuple.Length);
            People people = new People();
            people.WorkID =tuple[0].Trim();
            people.Name = tuple[1].Trim();
            people.ProjectGroup = tuple[2].Trim();
            people.Department = tuple[3].Trim();
     
            GameLogic.Instance().AddItemToList(people);
        }
    }
    
}

