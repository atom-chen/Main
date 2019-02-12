local ImpactTargetType = require("BattleCore/Common/ImpactTargetType")

require("BattleCore/SkillProcess/ScriptSkillParser")
local sc = require("ScriptSkill/ScriptSkillCommon")

local ImpactSubClass = require("BattleCore/Common/ImpactSubClass")
local ImpactTargetType = require("BattleCore/Common/ImpactTargetType")
local ImpactClass = require("BattleCore/Common/ImpactClass")

local i_Frozen = i_mk(sc.CommonBuffs.Frozen)           --引用通用冰冻
      i_Frozen.Duration = 1                            --修改持续回合 


local i_damage  = i_mk{
    ImpactLogic = 0,              --氐普通伤害
    Param_1 ="a1",                -- 攻击力百分比(无则填0   10000表示造成的100%攻击力)
    ImpactClass = 2,
}

local i_iffroze  = i_mk{
    ImpactLogic = 29,              --减速冰冻
    Param_1 =2,    
    Param_2 =1024,    
    Param_3 =1,
    Param_4 =i_Frozen,
}



local sk_main = sk_mk{    --氐觉醒3技能人界
    H_1 = h_mk{
        TargetType = 1,
        I_1 = {
            Impact = i_damage,
        },
        I_2 = {
            Impact = i_iffroze,
        },
    },


}
return sk_main


