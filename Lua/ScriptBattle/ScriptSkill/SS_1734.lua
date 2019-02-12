local ImpactTargetType = require("BattleCore/Common/ImpactTargetType")

require("BattleCore/SkillProcess/ScriptSkillParser")
local sc = require("ScriptSkill/ScriptSkillCommon")

local ImpactSubClass = require("BattleCore/Common/ImpactSubClass")
local ImpactTargetType = require("BattleCore/Common/ImpactTargetType")
local ImpactClass = require("BattleCore/Common/ImpactClass")





local i_damage1  = i_mk{
    ImpactLogic = 0,              --桃瑶普通伤害

    Param_1 ="a1",                -- 攻击力百分比(无则填0   10000表示造成的100%攻击力)
    Param_2 = 0,                  -- 额外固定值（0表示无附加掉血；如填60表示附加60点掉血）
    Param_3 = -1,                 -- 同一技能相同impact多次命中衰减系数
    ImpactClass = 2,
}

local i_damage2  = i_mk{           --桃瑶固定伤害
    ImpactLogic = 23,              -- 逻辑说明：根据技能释放者生命上限百分比直接造成伤害;无视防御，不会被属性和伤害放大修正

    Param_1 = 400,                 -- 参数1：百分比
    ImpactClass = 0,
}



local sk_main = sk_mk{
    H_1 = h_mk{
        TargetType = 1,        
        I_1 = {
            Impact = i_damage1,
        },
        I_2 = {
            Impact = i_damage2,
        },
    },


}
return sk_main


