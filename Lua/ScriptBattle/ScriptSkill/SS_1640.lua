local ImpactTargetType = require("BattleCore/Common/ImpactTargetType")

require("BattleCore/SkillProcess/ScriptSkillParser")
local sc = require("ScriptSkill/ScriptSkillCommon")

local ImpactSubClass = require("BattleCore/Common/ImpactSubClass")
local ImpactTargetType = require("BattleCore/Common/ImpactTargetType")
local ImpactClass = require("BattleCore/Common/ImpactClass")


local i_DefenceReduce = i_mk(sc.CommonBuffs.DefenceReduce)          --引用通用防御降低
i_DefenceReduce.Duration = 1                                        --修改持续回合


local i_suck = i_mk{
    ImpactLogic = 4,                      --吸血
    Param_1 = 101,
    Param_2 = 2000,
    Param_3 = 0,
    Duration =1,
    AliveCheckType = 2,
    AutoFadeOutTag = 32,
}


-- local i_defdown = i_mk{
--     Id = 902001,
--     Duration = 1,
-- }

local sk_defdown = sk_mk{
    H_1 = h_mk{
        TargetType=1,
        IsAnimHit = 0,
        I_1 = {
            Impact = i_DefenceReduce,
        },
    },
}


--普通伤害
local i_damage = i_mk{
    ImpactLogic = 0,                       --逻辑说明：普通伤害
    ImpactClass= 2,                        --效果分类（0，无类型，1增益，2减益（普通伤害），4减益）
    Param_1 = "a1",                        --攻击力百分比(无则填0   10000表示造成的100%攻击力)
    Duration =0,
}

--暴击降防
local i_critdefdown = i_mk{
    ImpactLogic = 44,                       --逻辑说明：属性修正
    Param_1 = sk_defdown,                           --//MaxHP = 0,//气血上限
    Duration =1,
    AliveCheckType = 2,
    AutoFadeOutTag = 16,
}


--普攻+暴击降防
local sk_main = sk_mk{
    H_1 = h_mk{
        TargetType= 2 ,
        IsAnimHit = 0,
        I_1 = {
            Impact = i_critdefdown,
        },
    },
    H_2 = h_mk{
        TargetType = 1,
        IsAnimHit = 1,
        I_1 = {
            Impact = i_damage,
        }  
    },


    
}
return sk_main

