
require("BattleCore/SkillProcess/ScriptSkillParser")
local sc = require("ScriptSkill/ScriptSkillCommon")

local ImpactSubClass = require("BattleCore/Common/ImpactSubClass")
local ImpactTargetType = require("BattleCore/Common/ImpactTargetType")
local ImpactClass = require("BattleCore/Common/ImpactClass")

--镜妖2技能：自爆

local i_damage  = i_mk{
    ImpactClass=2,
    ImpactLogic = 0,              --普通伤害

    Param_1 = "a1",               -- 攻击力百分比(无则填0   10000表示造成的100%攻击力)
    Param_2 = 0,                  -- 额外固定值（0表示无附加掉血；如填60表示附加60点掉血）
    Param_3 = -1,                 -- 同一技能相同impact多次命中衰减系数
}


local i_zibao  = i_mk{
    Duration=0,
    AliveCheckType=1,
    
    ImpactLogic = 3,              --普通伤害

    Param_1 = 0,               -- 攻击力百分比(无则填0   10000表示造成的100%攻击力)

}



local sk_main = sk_mk{

    

 
    H_1 = h_mk{                         --伤害
        TargetType = 1,
        I_1 = {
            Impact = i_damage,
        },
    },
    H_2 = h_mk{                         --伤害
        TargetType = 2,
        I_1 = {
            Impact = i_zibao,
    },
},


   
}

return sk_main