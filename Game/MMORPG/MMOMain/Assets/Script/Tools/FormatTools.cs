using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


class FormatTools
{
  public static void GetTimeFromInt(int timer,out int hour,out int min,out int second)
  {
    hour = timer /3600;
    timer = timer % 3600;
    min = timer / 60;
    timer = timer % 60;
    second = timer % 60;
  }
}

