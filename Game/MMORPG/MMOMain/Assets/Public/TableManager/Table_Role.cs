using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


class Table_Role
{
    //获取体力上限
    public static int GetEnergyLimit(int level)
    {
        int baseNum = 60;
        int increment = 1;
        return baseNum + level * increment;
    }
    public static uint GetEnergyLimit(uint level)
    {
        int baseNum = 60;
        int increment = 1;
        return (uint)(baseNum + level * increment);
    }

    //获取历练上限
    public static int GetToughenLimit(int level)
    {
        int baseNum = 60;
        int increment = 1;
        return baseNum + level * increment;
    }

    //获取历练上限
    public static uint GetToughenLimit(uint level)
    {
        int baseNum = 60;
        int increment = 1;
        return (uint)(baseNum + level * increment);
    }

    //获取经验上限
    public static int GetExpLimit(int level)
    {
        int baseNum = 90;
        int line = 20;
        int pow = 2;
        return (int)(baseNum + line * Math.Pow(level, pow));
    }

    //获取恢复一点所需时间:秒
    public static int GetRecoverSpendTime()
    {
        return 300;
    }
}

