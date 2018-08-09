local ImpactTargetType = require("BattleCore/Common/ImpactTargetType")

require("BattleCore/SkillProcess/ScriptSkillParser")
local sc = require("ScriptSkill/ScriptSkillCommon")

local ImpactSubClass = require("BattleCore/Common/ImpactSubClass")
local ImpactTargetType = require("BattleCore/Common/ImpactTargetType")
local ImpactClass = require("BattleCore/Common/ImpactClass")

--挂标记
local sk_addflag = sk_mk{
    H_1 = h_mk{
        TargetType=1,
        I_1 = {
            Impact =  i_mk{
                Id = 1274010,
                Duration = -1,
                ImpactLogic = -1,   
            }
        },
    },
}

--我方死人时
local i_frienddie = i_mk{                 
    ImpactLogic = 24,                      --逻辑说明：定期触发使用技能
    ImpactClass= 0,                        --效果分类（0，无类型，1增益，2减益（普通伤害），4减益）
    Param_1 = sk_addflag,                  --挂标记
    Param_2 = 6,                           --任意队友死亡时；
    Duration =-1,
}

--敌方死人时
local i_enemydie = i_mk{                 
    ImpactLogic = 24,                      --逻辑说明：定期触发使用技能
    ImpactClass= 0,                        --效果分类（0，无类型，1增益，2减益（普通伤害），4减益）
    Param_1 = sk_addflag,                  --挂标记
    Param_2 = 7,                           --任意敌人死亡时；
    Duration =-1,
}


--挂24号逻辑，死人时给自己加BUFF
local sk_main = sk_mk{
    H_1 = h_mk{
    TargetType=1,
            I_1 = {
                Impact = i_frienddie,
            },

            I_2 = {
                Impact = i_enemydie,
            },
    },
}

return sk_main