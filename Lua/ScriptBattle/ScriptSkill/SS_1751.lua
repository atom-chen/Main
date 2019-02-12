local ImpactTargetType = require("BattleCore/Common/ImpactTargetType")

require("BattleCore/SkillProcess/ScriptSkillParser")
local sc = require("ScriptSkill/ScriptSkillCommon")

local ImpactSubClass = require("BattleCore/Common/ImpactSubClass")
local ImpactTargetType = require("BattleCore/Common/ImpactTargetType")
local ImpactClass = require("BattleCore/Common/ImpactClass")

local i_Stun = i_mk(sc.CommonBuffs.Stun)    --引用通用晕眩
      i_Stun.Duration = 2                   --修改持续回合 


local i_damage1  = i_mk{
    ImpactLogic = 0,              --修蛇普通伤害
    Param_1 ="a1",                -- 攻击力百分比(无则填0   10000表示造成的100%攻击力)
    ImpactClass = 2,
}

local sk_main = sk_mk{
    H_1 = h_mk{
        TargetType = 1,
        I_1 = {
            Impact = i_mk{
                    ImpactLogic = 57,              --目标每1层DOT效果，伤害提升X%
                    Param_1 =3,   
                    Param_2 =64,
                    Param_3 =7,
                    Param_4 = -2000,              
                    Duration = 1,
                    AliveCheckType = 2,
                    AutoFadeOutTag = 16,
                },
        },
        I_2 = {
            Impact = i_damage1
                },
        I_3 = {
            Impact = i_mk{
                    ImpactLogic = 29,              --目标身上有超过4层效果，造成晕眩1回合
                    Param_1 =2,   
                    Param_2 =64,
                    Param_3 =4, 
                    Param_4 =i_Stun,           
                },
        },
    },

}
return sk_main


