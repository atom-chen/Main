local ImpactTargetType = require("BattleCore/Common/ImpactTargetType")

require("BattleCore/SkillProcess/ScriptSkillParser")
local sc = require("ScriptSkill/ScriptSkillCommon")

local ImpactSubClass = require("BattleCore/Common/ImpactSubClass")
local ImpactTargetType = require("BattleCore/Common/ImpactTargetType")
local ImpactClass = require("BattleCore/Common/ImpactClass")




--小籍为我方目标回复相当于目标最大生命值35%的血量，并为其施加持续2回合的防御强化和持续2回合的强韧效果。冷却时间5回合。


local i_recovery = i_mk{
    
    ImpactLogic =1,                         --治疗

    Param_1 = 3,                            --治疗类型(1攻击力,2治疗者血上限,3被治疗者血上限)
    Param_2 = "a1",                         --技能系数(10000表示造成的100%的治疗)
    Duration = 0,

}


--防御强化
local i_DefenceEnhance = i_mk(sc.CommonBuffs.DefenceEnhance)    
      i_DefenceEnhance.Duration = 2                      --修改持续回合    

--强韧
local i_CritChanceResist = i_mk(sc.CommonBuffs.CritChanceResist)    
      i_CritChanceResist.Duration = 2                      --修改持续回合          




local sk_main = sk_mk{
    

    
    H_1=h_mk{
           TargetType=1,
            I_1={                           -- 治疗
                Impact =i_recovery,
            },
            I_2={                           
            Impact = i_DefenceEnhance,
            },
            I_3={                           
                Impact = i_CritChanceResist,
            },
    }
 
} 
                      

return sk_main