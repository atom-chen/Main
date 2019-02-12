local ImpactTargetType = require("BattleCore/Common/ImpactTargetType")

require("BattleCore/SkillProcess/ScriptSkillParser")
local sc = require("ScriptSkill/ScriptSkillCommon")

local ImpactSubClass = require("BattleCore/Common/ImpactSubClass")
local ImpactTargetType = require("BattleCore/Common/ImpactTargetType")
local ImpactClass = require("BattleCore/Common/ImpactClass")


--牛魔王 1技能普攻+吸条
local i_damage = i_mk{
    ImpactClass=2,
    ImpactLogic = 0,                --普通伤害

    Param_1 = "a1",                 -- 攻击力百分比(无则填0   10000表示造成的100%攻击力)
    Param_2 = 0,                    -- 额外固定值（0表示无附加掉血；如填60表示附加60点掉血）
    Param_3 = 0,                    -- 同一技能相同impact多次命中衰减系数
}

local i_xitiao = i_mk{
    IsPassiveImpact=1,

    ImpactLogic = 55,               --行动条抽取  
    Param_1 = 200,                  --吸取量
}


local sk_main = sk_mk{

    H_1 = h_mk{
        TargetType = 4 ,
        I_1 = {                         --伤害
            Impact = i_damage,
        },
 
        I_2 = {                        --吸条
            Impact =i_xitiao,
        }
    
    },
    H_2 = h_mk{
        IsAnimHit =0,
        TargetType = 2 ,
        I_1 = {                         
            Impact = i_mk{  
                Id= 21010,
                Duration = -1,
                AliveCheckType = 2,
                AutoFadeOutTag=0,
                IsShow=1,
                ImpactClass=4,
                ImpactSubClass=0,
            
            
                ImpactLogic = 4,
                Param_1 = 3,
                Param_2 = -1,  
                Param_3 = 0,  
            },
        },

    },

    
}

return sk_main