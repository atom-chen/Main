local ImpactTargetType = require("BattleCore/Common/ImpactTargetType")

require("BattleCore/SkillProcess/ScriptSkillParser")
local sc = require("ScriptSkill/ScriptSkillCommon")

local ImpactSubClass = require("BattleCore/Common/ImpactSubClass")
local ImpactTargetType = require("BattleCore/Common/ImpactTargetType")
local ImpactClass = require("BattleCore/Common/ImpactClass")


local i_DefenceReduce = i_mk(sc.CommonBuffs.DefenceReduce)   --引用通用防御降低buff
      i_DefenceReduce.Duration = 2                            --修改持续回合 


local i_damage1  = i_mk{
    ImpactLogic = 0,              --素馨普通伤害
    Param_1 ="a1",         -- 攻击力百分比(无则填0   10000表示造成的100%攻击力)
    ImpactClass = 2,
}

-- local i_dispel  = i_mk{
--     ImpactLogic = 5,              --素馨驱散对方增益
--     Param_1 =1,   
--     Param_4 =1,
--     Param_5 =1,            
--     ImpactClass = 2,
-- }


local sk_main = sk_mk{
    H_1 = h_mk{
        TargetType = 1,
        I_1 = {
            IsChanceRefix = 1,
            Impact = i_DefenceReduce,
        },
        I_2 = {
            Impact = i_damage1,
        },

    },
}
return sk_main


