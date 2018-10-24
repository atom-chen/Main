using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public class Config
{
    private static string ConfigFilePath = "GameConfig.txt";     //配置文件路径

    public static int Max_Strengthen = (15);    //最大强化等级

    //读配置文件
    static Config()
    {
        LoadConfigFile();
    }

    //读取配置文件数据
    private static void LoadConfigFile()
    {

    }
}

