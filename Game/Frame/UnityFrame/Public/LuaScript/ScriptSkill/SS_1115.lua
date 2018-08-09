
require("BattleCore/SkillProcess/ScriptSkillParser")
local sc = require("ScriptSkill/ScriptSkillCommon")

local ImpactSubClass = require("BattleCore/Common/ImpactSubClass")
local ImpactTargetType = require("BattleCore/Common/ImpactTargetType")
local ImpactClass = require("BattleCore/Common/ImpactClass")

--筝2技能：濒死复活

local i_fuhuo  = i_mk{
    Duration = -1,
    ImpactClass= 0,
    CooldownId= 1115,
    Cooldown = 6,
    
    ImpactLogic = 13,              --死亡立即复活

    Param_1 = 99,                  -- 激活次数
    Param_2 = 3000,                -- 回复的血量百分比（10000）
    Param_3 = "a1",                 -- 复活后，额外放一个技能（当前回合行动）
}



local sk_main = sk_mk{

    

 
    H_1 = h_mk{                         --伤害
        TargetType = 2,
        I_1 = {
            Impact = i_fuhuo,
        },
    }

   
}

return sk_main