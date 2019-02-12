local ImpactTargetType = require("BattleCore/Common/ImpactTargetType")

local ss = require("BattleCore/SkillProcess/ScriptSkillParser")
local sc = require("ScriptSkill/ScriptSkillCommon")

local ImpactSubClass = require("BattleCore/Common/ImpactSubClass")
local ImpactTargetType = require("BattleCore/Common/ImpactTargetType")
local ImpactClass = require("BattleCore/Common/ImpactClass")

--修蛇1技能，妖界

--普通伤害
local i_damage = i_mk{
    ImpactLogic = 0,                       --逻辑说明：普通伤害
    ImpactClass= 2,                        --效果分类（0，无类型，1增益，2减益（普通伤害），4减益）
    Param_1 = "a1",                        --攻击力百分比(无则填0   10000表示造成的100%攻击力)
    Duration =0,
}
local i_Poison = i_mk(sc.CommonBuffs.Poison)   --引用通用持续伤害buff
      i_Poison.Duration = 1                      --修改持续回合              

--普攻
local sk_main = sk_mk{                 --妖界技能
    EnvLimit = 1 ,
            H_1 = ss.select_by_chance{
                {10000,h_mk{
                        TargetType=1,
                        I_1 = {
                            Impact = i_damage,
                        },
                
                        I_2 = {
                            Chance = "a2",
                            IsChanceRefix = 1,
                            Impact = i_Poison,
                        }  
                    }
                },
                {10000,h_mk{
                        TargetType=6,
                        TargetParam_1 = 3,
                        TargetParam_2 = 1,
                        I_1 = {
                            Impact = i_damage,
                        },
                
                        I_2 = {
                            Chance = 10000,
                            IsChanceRefix = 1,
                            Impact = i_Poison,
                        }  
                    }
                }
            }   
}

return sk_main

