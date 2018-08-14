using Public.TableManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public struct RoleAttr_BaseUp_Pair
{
    public int baseVal;
    public int upVal;
}
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

class TableRoleManager
{
    private static Dictionary<BATTLE_TYPE, Tab_Role> m_RoleDic = new Dictionary<BATTLE_TYPE, Tab_Role>();

    static TableRoleManager()
    {
        ReLoad();
    }

    public static void ReLoad()
    {
        try
        {
            m_RoleDic.Clear();
            ReadTableTools table = new ReadTableTools("Role.txt");
            //将每个元组构造成Tab_Item，添加到表格
            while (!table.IsTupleEnd())
            {
                string data;
                Tab_Role role = new Tab_Role();

                //ID
                data = table.GetData();
                role._Type = (BATTLE_TYPE)Convert.ToInt32(data);

                //力量
                data = table.GetNext();
                role.basePower = Convert.ToInt32(data);
                data = table.GetNext();
                role.upPower = Convert.ToInt32(data);

                //敏捷
                data = table.GetNext();
                role.baseAgility = Convert.ToInt32(data);
                data = table.GetNext();
                role.upAgility = Convert.ToInt32(data);

                //精神
                data = table.GetNext();
                role.baseSpirit = Convert.ToInt32(data);
                data = table.GetNext();
                role.upSpirit = Convert.ToInt32(data);

                //体力
                data = table.GetNext();
                role.baseLife = Convert.ToInt32(data);
                data = table.GetNext();
                role.upLife = Convert.ToInt32(data);

                //灵力
                data = table.GetNext();
                role.baseWakan = Convert.ToInt32(data);
                data = table.GetNext();
                role.upWakan = Convert.ToInt32(data);


                m_RoleDic.Add(role._Type, role);
                table.LineDown();
            }
        }
        catch(Exception)
        {

        }
    }

    //根据战魂师类别获取
    public static Tab_Role GetBattltTypeAttr(BATTLE_TYPE type)
    {
        Tab_Role ret = null;
        if(m_RoleDic.TryGetValue(type,out ret))
        {
            return ret;
        }
        return null;
    }



    //根据战魂师类别以及属性类别获取
    public static RoleAttr_BaseUp_Pair GetBattltTypeAttr(BATTLE_TYPE battleType, ATTR_TYPE attrType)
    {
        Tab_Role tabRole = null;
        RoleAttr_BaseUp_Pair ret=new RoleAttr_BaseUp_Pair();
        if (m_RoleDic.TryGetValue(battleType, out tabRole))
        {
            switch (attrType)
            {
                case ATTR_TYPE.POWER:
                    ret.baseVal = tabRole.basePower;
                    ret.upVal = tabRole.upPower;
                    break;
                case ATTR_TYPE.AGILITY:
                    ret.baseVal = tabRole.baseAgility;
                    ret.upVal = tabRole.upAgility;
                    break;
                case ATTR_TYPE.SPIRIT:
                    ret.baseVal = tabRole.baseSpirit;
                    ret.upVal = tabRole.upSpirit;
                    break;
                case ATTR_TYPE.LIFE:
                    ret.baseVal = tabRole.baseLife;
                    ret.upVal = tabRole.upLife;
                    break;
                case ATTR_TYPE.WAKAN:
                    ret.baseVal = tabRole.baseWakan;
                    ret.upVal = tabRole.upWakan;
                    break;
            }
        }
        return ret;
    }

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

