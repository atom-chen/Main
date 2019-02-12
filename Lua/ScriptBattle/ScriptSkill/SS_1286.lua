
require("BattleCore/SkillProcess/ScriptSkillParser")
local sc = require("ScriptSkill/ScriptSkillCommon")

local ImpactSubClass = require("BattleCore/Common/ImpactSubClass")
local ImpactTargetType = require("BattleCore/Common/ImpactTargetType")
local ImpactClass = require("BattleCore/Common/ImpactClass")

--祸斗觉醒3技能：【被动】祸斗使用两次技能后激活为可使用技能，对敌人造成祸斗攻击640%的普通伤害。根据激活之前的技能组合情况，将分别获得【黑炎】【赤炎】【幽炎】效果。冷却时间2回合。
--【人界效果】对生命低于50%的敌人额外使用1次伤害量为50%的当前技能。[A78C84]【妖界效果】对生命低于50%的敌人额外使用2次伤害量为50%的当前技能。[-]

local i_zuhe  = i_mk{
    Duration=-1,
    ImpactLogic = 42,              --组合技，监听1,2技能使用，通过组合替换3技能

    Param_1 = "a1",               -- 组合1,1后替换的技能id
    Param_2 = "a2",               -- 组合2,2后替换的技能id
    Param_3 = "a3",               -- 组合1,2后替换的技能id
    Param_4 = "a4",               --组合2,1后替换的技能id
    Param_5 = -1,                 --是否是baseid，（1，2，3，4参数都是BaseID，而不是exId）
}





local sk_main = sk_mk{

 
    H_1 = h_mk{                         --伤害
        TargetType =2,
        I_1 = {
            Impact = i_zuhe,
        }
    }
   
}

return sk_main