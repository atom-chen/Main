local ImpactTargetType = require("BattleCore/Common/ImpactTargetType")

require("BattleCore/SkillProcess/ScriptSkillParser")
local sc = require("ScriptSkill/ScriptSkillCommon")

local ImpactSubClass = require("BattleCore/Common/ImpactSubClass")
local ImpactTargetType = require("BattleCore/Common/ImpactTargetType")
local ImpactClass = require("BattleCore/Common/ImpactClass")


--玉圭3技能 拉条20%  狂暴两回合，急速两回合
local i_latiao = i_mk{
    ImpactLogic = 6,                --行动条增加减少

    Param_1 = "a1",                 -- 攻击力百分比(无则填0   10000表示造成的100%攻击力)
  
}
local i_CriticalEnhance = i_mk(sc.CommonBuffs.CriticalEnhance)   --引用通用狂暴
      i_CriticalEnhance.Duration = 2                            --修改持续回合       
local i_SpeedEnhance = i_mk(sc.CommonBuffs.SpeedEnhance)   --引用通用急速
      i_SpeedEnhance.Duration = 2                            --修改持续回合       

local sk_main = sk_mk{

    H_1 = h_mk{
        TargetType = 3 ,
        I_1 = {                         --拉条
            Impact = i_latiao,
        },
        I_2 = {                         --狂暴
            Impact = i_CriticalEnhance,
        },
        I_3 = {                         --急速
            Impact = i_SpeedEnhance,
        },
    },
    
}

return sk_main