local ImpactTargetType = require("BattleCore/Common/ImpactTargetType")

require("BattleCore/SkillProcess/ScriptSkillParser")
local sc = require("ScriptSkill/ScriptSkillCommon")

local ImpactSubClass = require("BattleCore/Common/ImpactSubClass")
local ImpactTargetType = require("BattleCore/Common/ImpactTargetType")
local ImpactClass = require("BattleCore/Common/ImpactClass")

--免疫DOT
local i_immune1 = i_mk{                 
    ImpactLogic = 7,                       --逻辑说明：免疫、抵消效果
    ImpactClass= 0,                        --效果分类（0，无类型，1增益，2减益（普通伤害），4减益）
    Param_1 = 0,                           --参数1：ImpactClass（0时不考虑，否则必须完全满足）
    Param_2 = 64,                          --参数2：ImpactSubClass（0时不考虑，否则和ImpactClass同时匹配时，抵消，满足任意一个SubClass即可）
    Param_3 = -1,                          --参数3：次数（-1时无限次数）
    Duration =-1,
}

local i_immune2 = i_mk{
    ImpactLogic = 7,                       --逻辑说明：免疫、抵消效果
    ImpactClass= 0,                        --效果分类（0，无类型，1增益，2减益（普通伤害），4减益）
    Param_1 = 0,                           --参数1：ImpactClass（0时不考虑，否则必须完全满足）
    Param_2 = 4096,                        --参数2：ImpactSubClass（0时不考虑，否则和ImpactClass同时匹配时，抵消，满足任意一个SubClass即可）
    Param_3 = -1,                          --参数3：次数（-1时无限次数）
    Duration =-1,
}

local sk_clean = sk_mk{
    H_1 = h_mk{
        TargetType=1,
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
                Duration = 0,
                ImpactLogic = 1,          --逻辑说明：治疗
                Param_1 = 3,              --参数1：参数1：治疗类型(1攻击力，2治疗者血上限，3被治疗者血上限)
                Param_2 = "a1",             --参数2：技能系数(10000表示造成的100%的治疗)
            }
        },
    },
}

--我方死人时
local i_frienddie = i_mk{                 
    ImpactLogic = 24,                      --逻辑说明：定期触发使用技能
    ImpactClass= 0,                        --效果分类（0，无类型，1增益，2减益（普通伤害），4减益）
    Param_1 = sk_clean,                  --挂标记
    Param_2 = 6,                           --任意队友死亡时；
    Duration =-1,
}

--敌方死人时
local i_enemydie = i_mk{                 
    ImpactLogic = 24,                      --逻辑说明：定期触发使用技能
    ImpactClass= 0,                        --效果分类（0，无类型，1增益，2减益（普通伤害），4减益）
    Param_1 = sk_clean,                    --挂标记
    Param_2 = 7,                           --任意敌人死亡时；
    Duration =-1,
}



--免疫DOT+免疫削减生命上限+每死个人驱散自己+回血
local sk_main = sk_mk{

    H_1 = h_mk{
        TargetType=1,

        I_1 = {
            Impact = i_immune1,
        },

        I_2 = {
            Impact = i_immune2,
        },   

        I_3 = {
            Impact = i_frienddie,
        },

        I_4 = {
            Impact = i_enemydie,
        },


    },

}

return sk_main