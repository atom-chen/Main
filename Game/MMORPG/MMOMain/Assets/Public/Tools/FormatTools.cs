using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public class FormatTools
{
    /// <summary>
    /// 解析MySQL传出的datetime
    /// </summary>
    public static void MySQLDateTimeToString(string dateVal, out int year, out int month, out int day, out int hour, out int min, out int second)
    {
        string[] words = dateVal.Split('/', ' ', ':', '\\');
        year = Convert.ToInt32(words[0]);
        month = Convert.ToInt32(words[1]);
        day = Convert.ToInt32(words[2]);
        hour = Convert.ToInt32(words[4]);
        min = Convert.ToInt32(words[5]);
        second = Convert.ToInt32(words[6]);
    }

    /// <summary>
    /// 从秒数获取详细时间
    /// </summary>
    /// <param name="timer">总秒数</param>
    /// <param name="hour">小时</param>
    /// <param name="min">分钟</param>
    /// <param name="second">秒</param>
    public static void GetTimeFromInt(int timer, out int hour, out int min, out int second)
    {
        hour = timer / 3600;
        timer = timer % 3600;
        min = timer / 60;
        timer = timer % 60;
        second = timer % 60;
    }
}

