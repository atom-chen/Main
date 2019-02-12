local ImpactTargetType = require("BattleCore/Common/ImpactTargetType")

require("BattleCore/SkillProcess/ScriptSkillParser")
local sc = require("ScriptSkill/ScriptSkillCommon")

local ImpactSubClass = require("BattleCore/Common/ImpactSubClass")
local ImpactTargetType = require("BattleCore/Common/ImpactTargetType")
local ImpactClass = require("BattleCore/Common/ImpactClass")



--给敌方全体放置２回合后引爆的降笔符咒，降笔符咒爆炸后造成扶鸾攻击300%的无视防御伤害，并使其受到持续1回合的眩晕效果。冷却时间5回合。【觉醒效果】每成功释放一个符咒效果，为自身增加10%的行动条。

local i_Stun = i_mk(sc.CommonBuffs.Stun)            --引用通用眩晕
      i_Stun.Duration = 1                           --修改持续回合   



local sk_main = sk_mk{

    H_1 = h_mk{
        TargetType=1,

        I_1 = {
            Impact = i_mk{
                Id = 1612010,
                ImpactLogic =37,
                Param_1 = 1,
                Param_2= 932,
                Param_3= "a1",
                Param_4 = -1,
                Param_5 = 10000,
                Param_6 = 1612011,
                Param_7 = 10000, 
                Param_8 = i_Stun,
                Param_9 = 10000,
                Duration = 2,
            },
            IsChanceRefix = 1,
        },
    },  
}
return sk_main

