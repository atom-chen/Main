
require("BattleCore/SkillProcess/ScriptSkillParser")
local sc = require("ScriptSkill/ScriptSkillCommon")

local ImpactSubClass = require("BattleCore/Common/ImpactSubClass")
local ImpactTargetType = require("BattleCore/Common/ImpactTargetType")
local ImpactClass = require("BattleCore/Common/ImpactClass")

--召唤河水攻击敌人，造成河伯生命最大值26%的普通伤害，使其受到持续2回合的缓速效果。


local i_damage  = i_mk{
    ImpactClass=2,
    ImpactLogic = 48,              --特殊伤害，根据指定的属性，计算攻击力

    Param_1 = "a1",                -- 技能系数
    Param_2 = 0,                   -- 技能固定值系数
    Param_3 = -1,                   -- 同一技能相同impact多次命中衰减系数
    Param_4 = 0,                    -- 属性1
    Param_5 = 10000,                -- 属性修正值1
    Param_6 = -1,                   -- 属性修正2
    Param_7 = -1,                   -- 属性修正2

}


--缓速效果
local i_SpeedReduce = i_mk(sc.CommonBuffs.SpeedReduce)     --引用缓速效果
      i_SpeedReduce.Duration = 2                            --修改持续回合          


local sk_main = sk_mk{
    H_1 = h_mk{

        TargetType = 1,
        I_1 = {
            Impact = i_damage,
        },
  
        I_2 = {
            IsChanceRefix = 1,
            Impact = i_SpeedReduce,
        }
    },

}



return sk_main