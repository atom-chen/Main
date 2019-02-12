local ImpactTargetType = require("BattleCore/Common/ImpactTargetType")

require("BattleCore/SkillProcess/ScriptSkillParser")
local sc = require("ScriptSkill/ScriptSkillCommon")

local ImpactSubClass = require("BattleCore/Common/ImpactSubClass")
local ImpactTargetType = require("BattleCore/Common/ImpactTargetType")
local ImpactClass = require("BattleCore/Common/ImpactClass")



--给敌方全体放置２回合后引爆的降笔符咒，降笔符咒爆炸后造成扶鸾攻击300%的无视防御伤害，并使其受到持续1回合的眩晕效果。冷却时间5回合。【觉醒效果】每成功释放一个符咒效果，为自身增加10%的行动条。




local sk_main = sk_mk{

    H_1 = h_mk{

        TargetType=2,

        I_1 = {
            Impact = i_mk{
                ImpactLogic = 6,                                             
                Param_1 = 100,                                 
                Duration = 0,
            },
        },
    },

}
return sk_main

