local ImpactTargetType = require("BattleCore/Common/ImpactTargetType")

require("BattleCore/SkillProcess/ScriptSkillParser")
local sc = require("ScriptSkill/ScriptSkillCommon")

local ImpactSubClass = require("BattleCore/Common/ImpactSubClass")
local ImpactTargetType = require("BattleCore/Common/ImpactTargetType")
local ImpactClass = require("BattleCore/Common/ImpactClass")

local i_Invincible = i_mk(sc.CommonBuffs.Invincible)   --引用通用无敌buff
      i_Invincible.Duration = 2                        --修改持续回合    
--为我方单体施加两回合无敌效果
local sk_main = sk_mk{

    H_1 = h_mk{
        TargetType = 1,
        I_1 = {
        Impact = i_Invincible,
        },
        I_2 = {
             Impact =1972010 ,                          --表现辅助
        },
    },
    -- H_2 = h_mk{
    --     TargetType = 1,
    --     I_1 = {
    --         Impact =1972010 ,                          --表现辅助
    --     },
    -- },


}

return sk_main

