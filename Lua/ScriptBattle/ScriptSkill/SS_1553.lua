
require("BattleCore/SkillProcess/ScriptSkillParser")
local sc = require("ScriptSkill/ScriptSkillCommon")

local ImpactSubClass = require("BattleCore/Common/ImpactSubClass")
local ImpactTargetType = require("BattleCore/Common/ImpactTargetType")
local ImpactClass = require("BattleCore/Common/ImpactClass")

--地动3技能觉醒：地动向地底多方位发射冲击炮攻击敌方全体，造成地动攻击240%的普通伤害，并随机交换敌人在行动条上的位置，同时以50%的概率使其受到持续2回合的缓速效果。冷却时间5回合。


local i_damage  = i_mk{
    ImpactClass=2,
    ImpactLogic = 0,              --普通伤害

    Param_1 = "a1",               -- 攻击力百分比(无则填0   10000表示造成的100%攻击力)
    Param_2 = 0,                  -- 额外固定值（0表示无附加掉血；如填60表示附加60点掉血）
    Param_3 = -1,                 -- 同一技能相同impact多次命中衰减系数
    
}


local i_xitiao  = i_mk{
    Duration = 0,
    MutexID=1552,
    MutexPriority=1,
    ImpactLogic = 52,              --行动条洗条，所有行动条上的人，互换位置

    Param_1 = -1,                

}


--缓速效果
local i_SpeedReduce = i_mk(sc.CommonBuffs.SpeedReduce)     --引用缓速效果
      i_SpeedReduce.Duration = 2                            --修改持续回合          


local sk_main = sk_mk{
    H_1 = h_mk{

        TargetType = 4,
        I_1 = {
            Impact = i_damage,
        },
        I_2 = {
            Impact = i_xitiao,
        },
        I_3 = {
            Chance = "a2",
            IsChanceRefix = 1,
            Impact = i_SpeedReduce,
        }
    },

}



return sk_main