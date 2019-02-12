local ImpactTargetType = require("BattleCore/Common/ImpactTargetType")

require("BattleCore/SkillProcess/ScriptSkillParser")
local sc = require("ScriptSkill/ScriptSkillCommon")

local ImpactSubClass = require("BattleCore/Common/ImpactSubClass")
local ImpactTargetType = require("BattleCore/Common/ImpactTargetType")
local ImpactClass = require("BattleCore/Common/ImpactClass")

--黑狼1技能
local i_Stun = i_mk(sc.CommonBuffs.Stun)     --引用通用眩晕buff
      i_Stun.Duration = 1                    --修改持续回合  
      
      
local i_Poison = i_mk(sc.CommonBuffs.Poison)   --引用通用中毒buff
      i_Poison.Duration = 3                    --修改持续回合   
--普通伤害



local i_damage = i_mk{
    ImpactLogic = 0,                       --逻辑说明：普通伤害
    ImpactClass= 2,                        --效果分类（0，无类型，1增益，2减益（普通伤害），4减益）
    Param_1 = "a1",                        --攻击力百分比(无则填0   10000表示造成的100%攻击力)
    Duration =0,
}

local i_AddStun = i_mk{                    --对有持续伤害效果的目标造成眩晕
    ImpactLogic = 29, 
    Param_1 = 2,
    Param_2 = 64,
    Param_3 = 1,
    Param_4 = i_Stun,

    Duration = 0,

}

--普攻
local sk_main = sk_mk{
    H_1 = h_mk{
        TargetType=1,
        I_1 = {
            Impact = i_damage,
        },
        I_2 = {
            Impact = i_AddStun 
            ,
        },
        I_3 = {
            Chance = "a2",
            IsChanceRefix = 1,
            Impact = i_Poison,
        }    
    },
}
return sk_main

