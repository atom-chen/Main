
require("BattleCore/SkillProcess/ScriptSkillParser")
local sc = require("ScriptSkill/ScriptSkillCommon")

local ImpactSubClass = require("BattleCore/Common/ImpactSubClass")
local ImpactTargetType = require("BattleCore/Common/ImpactTargetType")
local ImpactClass = require("BattleCore/Common/ImpactClass")

--筝2技能：无敌加血
local i_Invincible = i_mk(sc.CommonBuffs.Invincible)       --引用通用攻击降低效果
      i_Invincible.Duration = 1                              --修改持续回合    

local i_recovery = i_mk{
   
    ImpactLogic =1,                         --治疗

    Param_1 = 3,                            --治疗类型(1攻击力,2治疗者血上限,3被治疗者血上限)
    Param_2 = 3000,                         --技能系数(10000表示造成的100%的治疗)

}

local i_clear = i_mk{

    ImpactLogic = 5,                --驱散buff/debuff

    Param_1 = 4,                    --被驱散的impact class
    Param_2 = -1,                   --subCLass
    Param_3 = -1,                    --tag
    Param_4 = -1,                   --驱散的数量
    Param_5 = -1,                   --是否提示(0不提示,其他提示)
    Duration = 0,

}

local sk_main = sk_mk{
    H_1 = h_mk{                         
        TargetType = 2,
        I_1 = {
            Impact = i_clear,
        },
        I_2 = {
            Impact = i_recovery,
        },
        I_3 = {
            Impact = i_Invincible,
        },
  
    },
    IgnoreSelfState = 1, --处于眩晕等不可行动的状态时可释放
    IgnoreTargetDead = 1
}

return sk_main