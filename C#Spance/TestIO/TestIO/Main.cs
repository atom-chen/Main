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
		public const string TablePath = "F:\\WorkSpace\\InputStream\\DrawCard\\P3.txt";

		public static void InitPeopleList()
		{
			//char[] temp=new char[]{' '};
			string[] contents = File.ReadAllLines (TablePath,Encoding.Default);
			for(int i=0;i<contents.Length;i++)           //遍历每一行
			{
				string[] tuple = contents[i].Split('	');//按空格分割该元组
				Console.WriteLine("第"+i+"元组切割为"+tuple.Length+"个字符串");
			}
		}
		public static void Main (string[] args)
		{
			InitPeopleList ();

		}
	}
}
