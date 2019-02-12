
require("BattleCore/SkillProcess/ScriptSkillParser")
local sc = require("ScriptSkill/ScriptSkillCommon")

local ImpactSubClass = require("BattleCore/Common/ImpactSubClass")
local ImpactTargetType = require("BattleCore/Common/ImpactTargetType")
local ImpactClass = require("BattleCore/Common/ImpactClass")





local i_AddAtkOnKill = i_mk{                    --加10%基础伤害
    ImpactLogic = 4,  
    Param_1 = 6,
    Param_2 = 1000,
    Param_3 = 0,

    Duration = -1,
    AliveCheckType = 1,
    LayerID  = 2101,
    LayerMax = 50,
}

local i_biaoxian = i_mk{                    --加10%基础伤害
    Id = 2101000,
    ImpactLogic = -1,  

}







local sk_assist1 = sk_mk{                        --辅助技能，给自己加buff
    H_1 = h_mk{
        TargetType = 2,
        I_1 ={
            Impact =  i_AddAtkOnKill,               --增加10%基础伤害
        },
        I_2 ={
            Impact =  i_biaoxian,               --增加10%基础伤害
        },
      
    },
}





local  i_UseSkillRoundBegin = i_mk{  -- 回合开始
    ImpactLogic = 24,                --定期触发使用技能

    Param_1 = sk_assist1,            -- 技能id
    Param_2 = 1,                    -- 周期类型（见24号逻辑）
    Param_3 = -1,                   -- 条件类型（见24号逻辑）
    Param_4 = -1,                   -- 条件参数
    Param_5 = -1,                   -- 条件参数
    Param_6 = -1,                   -- 是否立即生效（回合开始时用的技能，必须时立即释放，这个参数无用，回合结束使用技能，必须时顺序释放，这个参数无用）
    Param_7 = -1,                   -- 技能类型（-1，skillExId；1，skillIndex）
    Param_8 = -1,                   -- 周期参数
    Param_9 = -1,                   -- 周期参数

    Duration = -1
}



local sk_main = sk_mk{

    H_1 = h_mk{                 
        TargetType = 2,
        I_1 = {
            Impact = i_UseSkillRoundBegin,      --回合开始
        },
    },
}

return sk_main