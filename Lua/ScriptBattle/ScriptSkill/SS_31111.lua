
require("BattleCore/SkillProcess/ScriptSkillParser")
local sc = require("ScriptSkill/ScriptSkillCommon")

local ImpactSubClass = require("BattleCore/Common/ImpactSubClass")
local ImpactTargetType = require("BattleCore/Common/ImpactTargetType")
local ImpactClass = require("BattleCore/Common/ImpactClass")

--攻击随机目标3次，分别造成小籍攻击170%的普通伤害，
--并分别对目标造成持续2回合的虚弱效果，持续2回合的防御降低效果，持续2回合的禁疗效果。冷却时间4回合。【妖界效果】所有的攻击将攻击指定目标。

local i_damage1  = i_mk{
    ImpactClass=2,
    ImpactLogic = 0,              --普通伤害

    Param_1 = "a1",               -- 攻击力百分比(无则填0   10000表示造成的100%攻击力)
    Param_2 = 0,                  -- 额外固定值（0表示无附加掉血；如填60表示附加60点掉血）
    Param_3 = -1,                 -- 同一技能相同impact多次命中衰减系数
}

local i_damage2  = i_mk{
    ImpactClass=2,
    ImpactLogic = 0,              --普通伤害

    Param_1 = "a1",               -- 攻击力百分比(无则填0   10000表示造成的100%攻击力)
    Param_2 = 0,                  -- 额外固定值（0表示无附加掉血；如填60表示附加60点掉血）
    Param_3 = -1,                 -- 同一技能相同impact多次命中衰减系数
}


local i_damage3  = i_mk{
    ImpactClass=2,
    ImpactLogic = 0,              --普通伤害

    Param_1 = "a1",               -- 攻击力百分比(无则填0   10000表示造成的100%攻击力)
    Param_2 = 0,                  -- 额外固定值（0表示无附加掉血；如填60表示附加60点掉血）
    Param_3 = -1,                 -- 同一技能相同impact多次命中衰减系数
}


--虚弱效果
local i_Weak = i_mk(sc.CommonBuffs.Weak)   
      i_Weak.Duration = 2                      --修改持续回合          

--防御降低
local i_DefenceReduce = i_mk(sc.CommonBuffs.DefenceReduce)    
      i_DefenceReduce.Duration = 2                      --修改持续回合    

--禁疗降低
local i_CureProhibit = i_mk(sc.CommonBuffs.CureProhibit)    
      i_CureProhibit.Duration = 2                      --修改持续回合          


local sk_main = sk_mk{

    
    H_1 = h_mk{
        TargetType=1,
        I_1 = {
            Impact = i_damage1,
        },
        I_2 = {
            IsChanceRefix = 1,
            Impact = i_Weak,
        },
    },
    H_2 = h_mk{
        TargetType=1,
        I_1 = {
            Impact = i_damage2,
        },
        I_2 = {
            IsChanceRefix = 1,
            Impact = i_DefenceReduce,
        },
    },
    H_3 = h_mk{
        TargetType=1,
        I_1 = {
            Impact = i_damage3,
        },
        I_2 = {
            IsChanceRefix = 1,
            Impact = i_CureProhibit,
        },
        
    },
 

}

return sk_main