
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

-- --睡眠效果
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

local i_attackup = i_mk{
    MutexID=1943,
    MutexPriority=1,

    ImpactLogic = 4,               
    Param_1 = 6,                    
    Param_2 = 5000,                    
    Param_3 = 0,
    Duration = 1,
    AliveCheckType=2,
    AutoFadeOutTag=0,
}





local sk_assist = sk_mk{                      
    H_1 = h_mk{
        TargetType = 2,
        I_1 ={
            Impact =  i_attackup,          
        },
      
    },  
}


local  i_UseSkillImpactFadeOut  = i_mk{  -- 睡眠被打破时使用技能
    ImpactLogic = 24,                    -- 定期触发使用技能

    Param_1 = sk_assist,                 -- 技能id
    Param_2 = 11,                        -- 周期类型（见24号逻辑）--发送的impact被移除
    Param_3 = -1,                        -- 条件类型（见24号逻辑）
    Param_4 = -1,                        -- 条件参数
    Param_5 = -1,                        -- 条件参数
    Param_6 = 1,                        -- 是否立即生效（回合开始时用的技能，必须时立即释放，这个参数无用，回合结束使用技能，必须时顺序释放，这个参数无用）
    Param_7 = -1,                        -- 技能类型（-1，skillExId；1，skillIndex）
    Param_8 = 1942010,                         -- 周期参数--impactId
    Param_9 =  2,                        -- 周期参数--移除类型，被移除

    Duration =1,
    MutexID = 1944,
    MutexPriority = 1,  
}



local sk_main = sk_mk{

    H_1 = h_mk{                         
        TargetType = 2,
        IsAnimHit = 0,
        I_1 = {
        Impact = i_UseSkillImpactFadeOut,
        },
    },

    H_2=h_mk{
           TargetType=2,
            I_1={                       --治疗
                Impact =i_recovery,
            },
            I_2={                       --睡眠
                Impact = i_sleep ,
            },
    },

        
} 
   


   
return sk_main