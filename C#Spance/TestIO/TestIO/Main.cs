using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Configuration;
using System.Text;

namespace TestIO
{
    class MainClass
    {
        private const string FileRoot = "C:\\MyWorkSpace\\InputStream\\DrawCard\\DrawCard\\";
        public const string TablePath = FileRoot + "p4.txt";
        public const string RememberPath = FileRoot + "Remember.txt";
        public static void InitPeopleList()
        {
            string[] contents = File.ReadAllLines(TablePath, Encoding.Default);          //读表1
            string names = File.ReadAllText(RememberPath, Encoding.UTF8);               //读表2
            Console.WriteLine(names);
            List<string> nameList = new List<string>(names.Split(' '));
            for (int i = 0; i < contents.Length; i++)           //遍历每一行
            {
                string[] tuple = contents[i].Split('	');//按空格分割该元组
                if (nameList.Contains(tuple[1]))
                {
                    Console.WriteLine(tuple[1] + "重复");
                }
            }
        }
        public static void Main(string[] args)
        {
            InitPeopleList();

        }
    }
}
