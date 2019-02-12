
require("BattleCore/SkillProcess/ScriptSkillParser")
local sc = require("ScriptSkill/ScriptSkillCommon")

local ImpactSubClass = require("BattleCore/Common/ImpactSubClass")
local ImpactTargetType = require("BattleCore/Common/ImpactTargetType")
local ImpactClass = require("BattleCore/Common/ImpactClass")

--酒童3技能：酒童喝酒回血，随后获得1回合的沉睡状态。

local i_recovery = i_mk{
    
    ImpactLogic =1,                         --治疗

    Param_1 = 3,                            --治疗类型(1攻击力,2治疗者血上限,3被治疗者血上限)
    Param_2 = "a1",                         --技能系数(10000表示造成的100%的治疗)
    Duration = 0,

}

--睡眠效果
-- local i_Sleep = i_mk(sc.CommonBuffs.Sleep)   --引用通用睡眠
--       i_Sleep.Duration = 1                    --修改持续回合          


local i_sleep = i_mk{ 
    Id = 1942010,
    ImpactLogic = -1,

    Duration = 1,
    AliveCheckType = 3,
    AutoFadeOutTag = 0,
    ImpactClass = 4,
    ImpactSubClass = 512, 
    MutexID = 1,
    MutexPriority =3,
}




local sk_main = sk_mk{

    H_1=h_mk{
           TargetType=2,
            I_1={                       --治疗
                Impact =i_recovery,
            },
            I_2={                       --睡眠
                Impact = i_sleep,
            },
    },

        
} 
   


   
return sk_main