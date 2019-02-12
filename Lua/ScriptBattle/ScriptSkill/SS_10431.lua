local ImpactTargetType = require("BattleCore/Common/ImpactTargetType")

require("BattleCore/SkillProcess/ScriptSkillParser")
local sc = require("ScriptSkill/ScriptSkillCommon")

local ImpactSubClass = require("BattleCore/Common/ImpactSubClass")
local ImpactTargetType = require("BattleCore/Common/ImpactTargetType")
local ImpactClass = require("BattleCore/Common/ImpactClass")

local i_Poison = i_mk(sc.CommonBuffs.Poison)     --引用通用持续伤害buff
      i_Poison.Duration = 2                      --修改持续回合   



local i_damage1  = i_mk{
    ImpactLogic = 0,              --夜莺普通伤害

    Param_1 ="a1",               -- 攻击力百分比(无则填0   10000表示造成的100%攻击力)
    Param_2 = 0,                  -- 额外固定值（0表示无附加掉血；如填60表示附加60点掉血）
    Param_3 = -1,                 -- 同一技能相同impact多次命中衰减系数
    ImpactClass = 2,
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
        }  
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
        }  
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
        }  
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
        }  
    },
    H_5 = h_mk{
        TargetType=2,
        IsAnimHit = 0,
        I_1 = {
            Impact =  i_mk{        
                Duration = 0,
                ImpactLogic = 5,          --逻辑说明：驱散buff/debuff
                Param_1 = 4,              --参数1：被驱散的impact class
                Param_4 = 99,             --参数4：驱散的数量
            }
        },
        I_2 = {
            Impact =  i_mk{
                Id=1042010,
                MutexID = 1042,
                MutexPriority =1,
                Duration = 1,
                
            }      
        },
    },



}
return sk_main


