
require("BattleCore/SkillProcess/ScriptSkillParser")
local sc = require("ScriptSkill/ScriptSkillCommon")

local ImpactSubClass = require("BattleCore/Common/ImpactSubClass")
local ImpactTargetType = require("BattleCore/Common/ImpactTargetType")
local ImpactClass = require("BattleCore/Common/ImpactClass")

--孟婆觉醒3技能

local i_recovery = i_mk{
    
    ImpactLogic =1,                         --治疗

    Param_1 = 2,                            --治疗类型(1攻击力,2治疗者血上限,3被治疗者血上限)
    Param_2 = "a1",                         --技能系数(10000表示造成的100%的治疗)
    Duration = 0,

}

local i_jianCD = i_mk{
    Duration = 0,
    
    
    ImpactLogic =35,                         --增减CD，有CD的技能，才会受这个效果影响

    Param_1 = -1,                            --技能索引（-1表示全部技能(1~3)）
    Param_2 = -1,                         --增减值(大于0增加CD，小于0减少CD)


}





--免疫效果
local i_Immune = i_mk(sc.CommonBuffs.Immune)   --引用通用免疫
      i_Immune.Duration = 2                      --修改持续回合          

--防御强化效果
local i_DefenceEnhance = i_mk(sc.CommonBuffs.DefenceEnhance)   --引用通用防御强化
      i_DefenceEnhance.Duration = 2                     --修改持续回合          



local sk_main = sk_mk{

    -- H_1=h_mk{
    --        TargetType=3,
    --         I_1={                       --治疗
    --             Impact =i_recovery,
    --         }
    --         I_2={                       --防御强化
    --             Impact =i_DefenceEnhance,
    --         },
    --       },
    -- EnvLimit = 0,
    -- OtherID = sk_mk{
        H_1=h_mk{
            TargetType=3,
             I_1={                       --治疗
                Impact =i_recovery,
             },
             I_2={                       --防御强化
             Impact =i_DefenceEnhance,
             },
             I_3={                       --免疫
                Impact =i_Immune,
             },
             I_4={                       --减CD
                Impact =i_jianCD,
          },
    },
         

    --     EnvLimit = 1,   
    -- },
        
} 
   


   
return sk_main