local ImpactTargetType = require("BattleCore/Common/ImpactTargetType")

require("BattleCore/SkillProcess/ScriptSkillParser")
local sc = require("ScriptSkill/ScriptSkillCommon")

local ImpactSubClass = require("BattleCore/Common/ImpactSubClass")
local ImpactTargetType = require("BattleCore/Common/ImpactTargetType")
local ImpactClass = require("BattleCore/Common/ImpactClass")


local i_Stun = i_mk(sc.CommonBuffs.Stun)            --引用通用眩晕
      i_Stun.Duration = 1                           --修改持续回合   

local i_Burning = i_mk(sc.CommonBuffs.Burning)      --引用通用灼伤
      i_Burning.Duration = 2                               --修改持续回合 

local i_damage1  = i_mk{
        ImpactLogic = 0,                                    -- 罗刹女普通伤害
        Param_1 ="a1",                                      -- 攻击力百分比(无则填0   10000表示造成的100%攻击力)
        ImpactClass = 2,
}

local i_atonce  = i_mk{
        ImpactLogic = 15,              --
        Param_1 =1,                 -- 
        Param_2 =7560,
        ImpactClass = 0,    
}


local sk_main = sk_mk{

    H_1 = h_mk{
        TargetType=1,

        I_1 = {
            Impact = i_damage1,
        },
        I_2 = {
            Impact = i_Burning,
        },
        I_3 = {
            Impact = i_mk{
                Id = 1612010,
                ImpactLogic =37,
                Param_1 = 1,
                Param_2= 932,
                Param_3= "a2",
                Param_4 = -1,
                Param_5 = 10000,
                Param_6 = 1612011,
                Param_7 = 10000, 
                Param_8 = i_Stun,
                Param_9 = 10000,
            },
            IsChanceRefix = 1,
        },
    },  
     
    H_2 = h_mk{
        TargetType=2,
        I_1 = {
            Impact = i_atonce,
        },
    }
}
return sk_main

