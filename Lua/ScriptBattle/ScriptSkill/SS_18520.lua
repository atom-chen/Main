local ImpactTargetType = require("BattleCore/Common/ImpactTargetType")

require("BattleCore/SkillProcess/ScriptSkillParser")
local sc = require("ScriptSkill/ScriptSkillCommon")

local ImpactSubClass = require("BattleCore/Common/ImpactSubClass")
local ImpactTargetType = require("BattleCore/Common/ImpactTargetType")
local ImpactClass = require("BattleCore/Common/ImpactClass")

local i_Poison = i_mk(sc.CommonBuffs.Poison)   --引用通用持续伤害buff
      i_Poison.Duration = 1                      --修改持续回合   



local i_damage1  = i_mk{
    ImpactLogic = 0,              --九尾灵狐普通伤害

    Param_1 ="a1",               -- 攻击力百分比(无则填0   10000表示造成的100%攻击力)
    Param_2 = 0,                  -- 额外固定值（0表示无附加掉血；如填60表示附加60点掉血）
    Param_3 = -1,                 -- 同一技能相同impact多次命中衰减系数
    ImpactClass = 2,
}

-- local i_ifpoison = i_mk{                       --
--     ImpactLogic = 29,                                             
--     Param_1 = 2,  
--     Param_2 = 64,
--     Param_3 = 1,
--     Param_4 = i_Poison,
--     Duration =0,
-- }


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
            Chance = "a2",
            IsChanceRefix = 1,
            Impact = i_Poison,
        },
    },

    H_2 = h_mk{
        TargetType=6,
        TargetParam_1 = 1,
        TargetParam_2 = 0,
        I_1 = {
            Impact = i_damage1,
        },
        I_2 = {
            Chance = "a2",
            IsChanceRefix = 1,
            Impact = i_Poison,
        },
    },
    H_3 = h_mk{
        TargetType=6,
        TargetParam_1 = 1,
        TargetParam_2 = 0,
        I_1 = {
            Impact = i_damage1,
        },
        I_2 = {
            Chance = "a2",
            IsChanceRefix = 1,
            Impact = i_Poison,
        },
    },
    H_4 = h_mk{
        TargetType=6,
        TargetParam_1 = 1,
        TargetParam_2 = 0,
        I_1 = {
            Impact = i_damage1,
        },
        I_2 = {
            Chance = "a2",
            IsChanceRefix = 1,
            Impact = i_Poison,
        },
    },
    H_5 = h_mk{
        TargetType=6,
        TargetParam_1 = 1,
        TargetParam_2 = 0,
        I_1 = {
            Impact = i_damage1,
        },
        I_2 = {
            Chance = "a2",
            IsChanceRefix = 1,
            Impact = i_Poison,
        },
    },
    H_6 = h_mk{
        TargetType=6,
        TargetParam_1 = 1,
        TargetParam_2 = 0,
        I_1 = {
            Impact = i_damage1,
        },
        I_2 = {
            Chance = "a2",
            IsChanceRefix = 1,
            Impact = i_Poison,
        },
    },
    H_7 = h_mk{
        TargetType=6,
        TargetParam_1 = 1,
        TargetParam_2 = 0,
        I_1 = {
            Impact = i_damage1,
        },
        I_2 = {
            Chance = "a2",
            IsChanceRefix = 1,
            Impact = i_Poison,
        },
    },
    H_8 = h_mk{
        TargetType=6,
        TargetParam_1 = 1,
        TargetParam_2 = 0,
        I_1 = {
            Impact = i_damage1,
        },
        I_2 = {
            Chance = "a2",
            IsChanceRefix = 1,
            Impact = i_Poison,
        },
    },
    H_9 = h_mk{
        TargetType=6,
        TargetParam_1 = 1,
        TargetParam_2 = 0,
        I_1 = {
            Impact = i_damage1,
        },
        I_2 = {
            Chance = "a2",
            IsChanceRefix = 1,
            Impact = i_Poison,
        },
    },
    H_10 = h_mk{
        TargetType=2,
        IsAnimHit = 0,
        I_1 = {
            Impact = i_atonce,
        },
    },
}
return sk_main


