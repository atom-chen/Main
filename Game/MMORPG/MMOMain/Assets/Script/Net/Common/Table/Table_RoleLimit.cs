using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


class Table_RoleLimit
{
  //获取体力上限
  public static uint GetEnergyLimit(uint level)
  {
    uint baseNum=60;
    uint increment=1;
    return baseNum + level * increment;
  }

  //获取历练上限
  public static uint GetToughenLimit(uint level)
  {
    uint baseNum = 60;
    uint increment = 1;
    return baseNum + level * increment;
  }

  //获取经验上限
  public static uint GetExpLimit(uint level)
  {
    int baseNum = 90;
    int line = 20;
    int pow = 2;
    return (uint)(baseNum + line * Math.Pow(level, pow));
  }
}

