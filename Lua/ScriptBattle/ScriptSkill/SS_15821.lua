local ImpactTargetType = require("BattleCore/Common/ImpactTargetType")

require("BattleCore/SkillProcess/ScriptSkillParser")
local sc = require("ScriptSkill/ScriptSkillCommon")

local ImpactSubClass = require("BattleCore/Common/ImpactSubClass")
local ImpactTargetType = require("BattleCore/Common/ImpactTargetType")
local ImpactClass = require("BattleCore/Common/ImpactClass")


--河伯用水流助阵我方任一符灵，使其立即获得1个行动回合，并为其施加持续2回合的攻击强化效果。【人界效果】为其回复30%的生命。冷却时间4回合。

local i_getround = i_mk{

    Duration=0,
    AliveCheckType=1,

    ImpactLogic =15,                 --下回合立即行动       

    Param_1= 1,                      --是否无视主角（1无视，0不无视）
    Param_2= 7560,                   --冒字的字典号

}



--攻击强化效果
local i_AttackEnhance = i_mk(sc.CommonBuffs.AttackEnhance)     --引用攻击强化效果
      i_AttackEnhance.Duration = 2                             --修改持续回合          


local sk_main = sk_mk{

    H_1 = h_mk{
        TargetType = 1,
        I_1 = {
            Impact = i_getround,
            
        },
        I_2 = {
            Impact =  i_AttackEnhance,
            
        },
     
    }
  


}

return sk_main