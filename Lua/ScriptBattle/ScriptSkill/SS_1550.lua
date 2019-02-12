
require("BattleCore/SkillProcess/ScriptSkillParser")
local sc = require("ScriptSkill/ScriptSkillCommon")

local ImpactSubClass = require("BattleCore/Common/ImpactSubClass")
local ImpactTargetType = require("BattleCore/Common/ImpactTargetType")
local ImpactClass = require("BattleCore/Common/ImpactClass")

--地动1技能：普攻+溅射

local i_damage  = i_mk{
    ImpactClass=2,
    ImpactLogic = 0,              --普通伤害

    Param_1 = "a1",               -- 攻击力百分比(无则填0   10000表示造成的100%攻击力)
    Param_2 = 0,                  -- 额外固定值（0表示无附加掉血；如填60表示附加60点掉血）
    Param_3 = -1,                 -- 同一技能相同impact多次命中衰减系数
    
}

--溅射
local i_jianshe  = i_mk{
    Duration = 1,
    AliveCheckType=2,
    AutoFadeOutTag=16,
    ImpactClass=2,
    ImpactLogic = 59,              --伤害溅射，产生伤害后，按照比例溅射给敌方2其他人

    Param_1 = 5000,                -- 溅射伤害比例
    Param_2 = -1,                  -- 暴击才触发
    Param_3 = -1,                  --溅射数量，-1表示所有人
}



local sk_main = sk_mk{
    H_1 = h_mk{
        IsAnimHit=0,
        TargetType = 2,
        I_1 = {
            Chance = "a2",
            Impact = i_jianshe,
   
        },
    },


    H_2 = h_mk{                         --伤害
        TargetType = 1,
        I_1 = {
            Impact = i_damage,
   
        }
    }
    
}



return sk_main