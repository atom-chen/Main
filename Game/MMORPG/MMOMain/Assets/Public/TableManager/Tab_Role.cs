using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public class Tab_Role
{
    public BATTLE_TYPE _Type;

    public int basePower;
    public int upPower;

    public int baseAgility;
    public int upAgility;

    public int baseSpirit;
    public int upSpirit;

    public int baseLife;
    public int upLife;

    public int baseWakan;
    public int upWakan;
}

class Table_RoleManager
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

