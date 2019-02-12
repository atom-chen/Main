local ImpactTargetType = require("BattleCore/Common/ImpactTargetType")

require("BattleCore/SkillProcess/ScriptSkillParser")
local sc = require("ScriptSkill/ScriptSkillCommon")

local ImpactSubClass = require("BattleCore/Common/ImpactSubClass")
local ImpactTargetType = require("BattleCore/Common/ImpactTargetType")
local ImpactClass = require("BattleCore/Common/ImpactClass")


--玉圭1技能 伤害+驱散buff
local i_damage = i_mk{
    ImpactClass=2,
    ImpactLogic = 0,                --普通伤害

    Param_1 = "a1",                 -- 攻击力百分比(无则填0   10000表示造成的100%攻击力)
    Param_2 = 0,                    -- 额外固定值（0表示无附加掉血；如填60表示附加60点掉血）
    Param_3 = 0,                    -- 同一技能相同impact多次命中衰减系数
}


local i_qusan= i_mk{
    ImpactLogic = 5,                --驱散buff
    
    Param_1 = 1,                    --被驱散的impact class
    Param_2 = -1,                   --subclasss
    Param_3 = -1,                   --tag
    Param_4 = 1,                    --驱散数量
    Param_5 = 1                     --是否提示
}   


local sk_main = sk_mk{

    H_1 = h_mk{
        TargetType = 1 ,
        I_1 = {                         --伤害
            Impact = i_damage,
        },
 
        I_2 = {                        --驱散
            Chance = "a2",
            Impact =i_qusan,
        }
    
    },
    
}

return sk_main