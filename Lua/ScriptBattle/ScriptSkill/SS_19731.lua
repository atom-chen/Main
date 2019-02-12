local ImpactTargetType = require("BattleCore/Common/ImpactTargetType")

require("BattleCore/SkillProcess/ScriptSkillParser")
local sc = require("ScriptSkill/ScriptSkillCommon")

local ImpactSubClass = require("BattleCore/Common/ImpactSubClass")
local ImpactTargetType = require("BattleCore/Common/ImpactTargetType")
local ImpactClass = require("BattleCore/Common/ImpactClass")

local i_hudun = i_mk{ --护盾
    ImpactClass= 1,
    ImpactSubClass=2048,
    Duration =2,
    AliveCheckType =3,
    Id = 1971010,

    ImpactLogic = 9, --护盾
    Param_1 = "a1",  --护盾比例
    Param_2 = 0,     ---1取接受者血量上限，0取施法者血量上限   
    MutexID = 1971,
    MutexPriority = 1,
}

local i_xingdongtiao = i_mk{
    ImpactLogic = 6,                          --行动条增加
    Param_1 = 150, 
}

local sk_assist = sk_mk{                      --辅助技能，为自己增加行动条
    H_1 = h_mk{
        TargetType = 2,
        I_1 ={
            Impact =  i_xingdongtiao,          --行动条增加
        },
      
    },  
}


local  i_UseSkillImpactFadeOut  = i_mk{  -- 护盾被打破时使用技能
    ImpactLogic = 24,                    -- 定期触发使用技能

    Param_1 = sk_assist,                 -- 技能id
    Param_2 = 11,                        -- 周期类型（见24号逻辑）--发送的impact被移除
    Param_3 = -1,                        -- 条件类型（见24号逻辑）
    Param_4 = -1,                        -- 条件参数
    Param_5 = -1,                        -- 条件参数
    Param_6 = 1,                        -- 是否立即生效（回合开始时用的技能，必须时立即释放，这个参数无用，回合结束使用技能，必须时顺序释放，这个参数无用）
    Param_7 = -1,                        -- 技能类型（-1，skillExId；1，skillIndex）
    Param_8 = 1971010,                         -- 周期参数--impactId
    Param_9 =  2,                        -- 周期参数--移除类型，被移除

    Duration =-1,
    MutexID = 1972,
    MutexPriority = 1,  
}
local i_Immune = i_mk(sc.CommonBuffs.Immune)   --引用通用免疫buff
      i_Immune.Duration = 2                      --修改持续回合        

--为我方全体施加护盾
local sk_main = sk_mk{
    
    H_1 = h_mk{                          --护盾被移除时，增加行动条
        TargetType = 2,
        IsAnimHit = 0,
        I_1 = {
        Impact = i_UseSkillImpactFadeOut,
        },
    },

    H_2 = h_mk{                          --全体护盾
        TargetType = 3,
        I_1 = {
            Impact = i_hudun,
        },
        I_2 = {
            Impact = i_Immune,
        },
    },

}

return sk_main
