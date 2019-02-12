local ImpactTargetType = require("BattleCore/Common/ImpactTargetType")

require("BattleCore/SkillProcess/ScriptSkillParser")
local sc = require("ScriptSkill/ScriptSkillCommon")

local ImpactSubClass = require("BattleCore/Common/ImpactSubClass")
local ImpactTargetType = require("BattleCore/Common/ImpactTargetType")
local ImpactClass = require("BattleCore/Common/ImpactClass")


--酒童辅助技能：在接下来的3回合造成嘲讽效果的概率提高30%，但是所有的攻击都会攻击随机目标，并对目标外的随机敌人造成80%的溅射伤害。

local i_damage  = i_mk{
    ImpactClass=2,
    ImpactLogic = 48,              --特殊伤害，根据指定的属性，计算攻击力

    Param_1 = "a1",                -- 技能系数
    Param_2 = 0,                   -- 技能固定值系数
    Param_3 = -1,                   -- 同一技能相同impact多次命中衰减系数
    Param_4 = 0,                    -- 属性1
    Param_5 = 10000,                -- 属性修正值1
    Param_6 = -1,                   -- 属性修正2
    Param_7 = -1,                   -- 属性修正2

}


--嘲讽效果
local i_Defiance = i_mk(sc.CommonBuffs.Defiance)   --引用通用嘲讽
      i_Defiance.Duration = 1                      --修改持续回合          

--溅射
local i_jianshe  = i_mk{
    Duration = 1,
    AliveCheckType=2,
    AutoFadeOutTag=16,
    ImpactClass=2,
    ImpactLogic = 59,              --伤害溅射，产生伤害后，按照比例溅射给敌方2其他人

    Param_1 = 8000,                -- 溅射伤害比例
    Param_2 = -1,                  -- 暴击才触发
    Param_3 = 1,                  --溅射数量，-1表示所有人
}


local sk_main = sk_mk{
    H_1 = h_mk{
        IsAnimHit=0,
        TargetType = 2,
        I_1 = {

            Impact = i_jianshe,
   
        },
    },


    H_2 = h_mk{                         --伤害
        TargetType=6,
        TargetParam_1 = 1,
        TargetParam_2 = 0,
        I_1 = {
            Impact = i_damage,
   
        },
        I_2 = {
            Chance = "a2",
            IsChanceRefix = 1,
            Impact = i_Defiance,
   
        }
    }
}
return sk_main


