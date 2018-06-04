using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class FormatTools
{
  /// <summary>
  /// 解析MySQL传出的datetime
  /// </summary>
  public static void MySQLDateTimeToString(string dateVal, out int year, out int month, out int day, out int hour, out int min, out int second)
  {
      string[] words = dateVal.Split('/', ' ', ':','\\');
      year = Convert.ToInt32(words[0]);
      month = Convert.ToInt32(words[1]);
      day = Convert.ToInt32(words[2]);
      hour = Convert.ToInt32(words[4]);
      min = Convert.ToInt32(words[5]);
      second = Convert.ToInt32(words[6]);
  }
}

