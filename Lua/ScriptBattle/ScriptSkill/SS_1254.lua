
require("BattleCore/SkillProcess/ScriptSkillParser")
local sc = require("ScriptSkill/ScriptSkillCommon")

local ImpactSubClass = require("BattleCore/Common/ImpactSubClass")
local ImpactTargetType = require("BattleCore/Common/ImpactTargetType")
local ImpactClass = require("BattleCore/Common/ImpactClass")

--海东青觉醒3技能：海东青万箭齐发攻击敌方全体，造成海东青攻击350%的普通伤害，每击杀一个敌人将回复自身15%的行动条。冷却时间5回合。


local i_damage  = i_mk{
    ImpactClass=2,
    ImpactLogic = 0,              --普通伤害

    Param_1 = "a1",               -- 攻击力百分比(无则填0   10000表示造成的100%攻击力)
    Param_2 = 0,                  -- 额外固定值（0表示无附加掉血；如填60表示附加60点掉血）
    Param_3 = -1,                 -- 同一技能相同impact多次命中衰减系数
}



-- 拉条
local i_actionbar = i_mk{
    Duration = 0,
    ImpactClass=1,

    ImpactLogic =6,                 --行动条增加减少           

    Param_1= 150,                  --增减数（负数表示减少）
   

}


local sk_jisha = sk_mk{
    H_1 = h_mk{

        TargetType = 2,
        I_1 ={
            
            Impact = i_actionbar ,               --获得回合
        },   
    }
   
}



local  i_UseSkilljisha = i_mk{       -- 击杀后释放技能
    Duration=1,
    AliveCheckType=2,
    IsShow=0,
    MutexID=1254,
    MutexPriority=1,
   
    ImpactLogic = 24,                --定期触发使用技能

    Param_1 = sk_jisha,            -- 技能id
    Param_2 = 8,                    -- 周期类型（见24号逻辑）
    Param_3 = -1,                   -- 条件类型（见24号逻辑）
    Param_4 = -1,                   -- 条件参数
    Param_5 = -1,                   -- 条件参数
    Param_6 = -1,                   -- 是否立即生效（回合开始时用的技能，必须时立即释放，这个参数无用，回合结束使用技能，必须时顺序释放，这个参数无用）
    Param_7 = -1,                   -- 技能类型（-1，skillExId；1，skillIndex）
    Param_8 = -1,                   -- 周期参数
    Param_9 = -1,                   -- 周期参数


}



local sk_main = sk_mk{

    H_1 = h_mk{                 --击杀后使用技能
        TargetType = 2,
        I_1 = {
            Impact = i_UseSkilljisha,
        },
    },
    H_2 = h_mk{                         --伤害
        TargetType = 4,
 
        I_1 = {
            Impact = i_damage,
        },
    },
}

return sk_main