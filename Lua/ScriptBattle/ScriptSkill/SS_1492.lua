
require("BattleCore/SkillProcess/ScriptSkillParser")
local sc = require("ScriptSkill/ScriptSkillCommon")

local ImpactSubClass = require("BattleCore/Common/ImpactSubClass")
local ImpactTargetType = require("BattleCore/Common/ImpactTargetType")
local ImpactClass = require("BattleCore/Common/ImpactClass")

--应龙3技能  发动一次群体攻击，目标血量高于50%时，伤害增加50%

local i_kezhi  = i_mk{
    Duration=-1,
    AutoFadeOutTag=64,
    Tag= 10004,

    ImpactLogic = -1, 
}

local i_damage  = i_mk{


    ImpactClass=2,
    ImpactLogic = 0,              --普通伤害

    Param_1 = "a1",               -- 攻击力百分比(无则填0   10000表示造成的100%攻击力)
    Param_2 = 0,                  -- 额外固定值（0表示无附加掉血；如填60表示附加60点掉血）
    Param_3 = -1,                 -- 同一技能相同impact多次命中衰减系数
}
     
--攻击力提升
local i_attackup = i_mk{
    MutexID=1492,
    MutexPriority=1,

    ImpactLogic = 4,               
    Param_1 = 7,                    
    Param_2 = -4000,                    
    Param_3 = 0,
    Duration = -1,
    AliveCheckType=1,
    AutoFadeOutTag=1,
}


local i_xueliangjiance = i_mk{
    ImpactLogic = 28,               
    Param_1 = 3,                    
    Param_2 = 8000,                    
    Param_3 = 1,
    Param_4 = i_attackup,

    Duration = 0,

}



local sk_main = sk_mk{
 
    H_1 = h_mk{
        IsAnimHit=0,                         --环境克制
        TargetType = 2,
            I_1 = {
                Impact = i_kezhi,
            }
    },

    H_2 = h_mk{                         --伤害
        TargetType = 4,
            I_1 = {
                Impact = i_xueliangjiance,
            },
            I_2 = {
                Impact = i_damage,
            }
    },

   
}

return sk_main





