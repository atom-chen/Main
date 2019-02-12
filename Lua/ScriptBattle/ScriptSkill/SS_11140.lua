
require("BattleCore/SkillProcess/ScriptSkillParser")
local sc = require("ScriptSkill/ScriptSkillCommon")

local ImpactSubClass = require("BattleCore/Common/ImpactSubClass")
local ImpactTargetType = require("BattleCore/Common/ImpactTargetType")
local ImpactClass = require("BattleCore/Common/ImpactClass")

--筝3技能：【人界效果】由女子连续攻击敌方全体3次，前两击造成筝攻击75%的普通伤害，第三击造成筝攻击150%的普通伤害，并为我方全体施加持续2回合的攻击强化效果。
--[A78C84]【妖界效果】由男子攻击敌方全体，造成筝攻击300%的普通伤害，并以80%的概率使敌方全体受到持续2回合的防御降低效果。[-]冷却时间5回合。



--攻击强化效果
local i_AttackEnhance = i_mk(sc.CommonBuffs.AttackEnhance)   --引用通用攻击强化
      i_AttackEnhance.Duration = 2                           --修改持续回合          


--一击
local i_damage1  = i_mk{
    ImpactClass=2,
    ImpactLogic = 0,              --普通伤害

    Param_1 = "a1",               -- 攻击力百分比(无则填0   10000表示造成的100%攻击力)
    Param_2 = 0,                  -- 额外固定值（0表示无附加掉血；如填60表示附加60点掉血）
    Param_3 = -1,                 -- 同一技能相同impact多次命中衰减系数
}

--二击
local i_damage2  = i_mk{
    ImpactClass=2,
    ImpactLogic = 0,              --普通伤害

    Param_1 = "a2",               -- 攻击力百分比(无则填0   10000表示造成的100%攻击力)
    Param_2 = 0,                  -- 额外固定值（0表示无附加掉血；如填60表示附加60点掉血）
    Param_3 = -1,                 -- 同一技能相同impact多次命中衰减系数
}


--三击
local i_damage3  = i_mk{
    ImpactClass=2,
    ImpactLogic = 0,              --普通伤害

    Param_1 = "a3",               -- 攻击力百分比(无则填0   10000表示造成的100%攻击力)
    Param_2 = 0,                  -- 额外固定值（0表示无附加掉血；如填60表示附加60点掉血）
    Param_3 = -1,                 -- 同一技能相同impact多次命中衰减系数
}


local sk_main = sk_mk{

    H_1 = h_mk{                         
        TargetType = 3,
    
        I_1 = {                        --攻击强化效果
            Impact = i_AttackEnhance,
        },
    },

 
    H_2 = h_mk{                         --1段伤害
        TargetType = 4,
        I_1 = {
            Impact = i_damage1,
        },
    },
    
    H_3 = h_mk{                         --2段伤害
    TargetType = 4,
    I_1 = {
        Impact = i_damage2,
    },
},

    H_4 = h_mk{                         --3段伤害
        TargetType = 4,
        I_1 = {
            Impact = i_damage3,
        },
    },
    H_5 = h_mk{
        IsAnimHit =0,
        TargetType = 2 ,
        I_1 = {                         
            Impact = i_mk{  
                Id= 21010,
                Duration = -1,
                AliveCheckType = 2,
                AutoFadeOutTag=0,
                IsShow=1,
                ImpactClass=4,
                ImpactSubClass=0,
            
            
                ImpactLogic = 4,
                Param_1 = 3,
                Param_2 = -1,  
                Param_3 = 0,  
            },
        },

    },

   
}

return sk_main