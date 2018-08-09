
require("BattleCore/SkillProcess/ScriptSkillParser")
local sc = require("ScriptSkill/ScriptSkillCommon")

local ImpactSubClass = require("BattleCore/Common/ImpactSubClass")
local ImpactTargetType = require("BattleCore/Common/ImpactTargetType")
local ImpactClass = require("BattleCore/Common/ImpactClass")

--孟婆召唤壶技能

--加cd
local i_jiaCD = i_mk{
    Duration = 0,
    IsShow=1,
    ImpactClass=4,
    ImpactLogic =35,                         --增减CD，有CD的技能，才会受这个效果影响

    Param_1 = -1,                            --技能索引（-1表示全部技能(1~3)）
    Param_2 = 1,                         --增减值(大于0增加CD，小于0减少CD)


}


--攻击降低效果
local i_AttackReduce = i_mk(sc.CommonBuffs.AttackReduce)   --引用通用攻击降低
      i_AttackReduce.Duration = 1                     --修改持续回合          



local sk_main = sk_mk{
    H_1 = h_mk{
        IsChanceRefix = 1,
        TargetType = 1,
        I_1 ={
            Impact =  i_AttackReduce,               --攻击降低
        }
    },
    H_2 = h_mk{
        TargetType = 1,
        I_1 ={
            Impact =  i_jiaCD,               --加CD
        }
    }
    
}
    
return sk_main