
require("BattleCore/SkillProcess/ScriptSkillParser")
local sc = require("ScriptSkill/ScriptSkillCommon")

local ImpactSubClass = require("BattleCore/Common/ImpactSubClass")
local ImpactTargetType = require("BattleCore/Common/ImpactTargetType")
local ImpactClass = require("BattleCore/Common/ImpactClass")

--孟婆2技能：召唤

local i_zhaohuancunhuo = i_mk{
    Duration=3,
    AliveCheckType=2,
    IsShow=1,
    ImpactClass=0,
    ImpactLogic = 41,              --buff消散时删除持有者

    Param_1 = -1,                  

  }

local i_zhaohuan  = i_mk{
    Duration=0,
    ImpactClass=0,
    ImpactLogic = 40,              --召唤

    Param_1 = 1,                  -- 召唤物id（召唤表的id）
    Param_2 = -1,                 -- 召唤区域（受召唤技能规则影响,0蓝方，1蓝方召唤，2红方，3红方召唤，4中立，5蓝方主角，6红方主角）
    Param_3 = -1,                 -- 召唤站位（受召唤技能规则影响）
    Param_4 = 0,                 -- 召唤规则（0有人则失败；1有人则替换；2无论是否有人都会召唤）
    Param_5 = i_zhaohuancunhuo,              -- 附加impact
    Param_6 = -1,                 -- 召唤数量(小于0表示用光位置（如果召唤类型是3任意位置召唤，则小于0无效），大于0表示指定数量)
    Param_7 = -1,                 -- 随机召唤起始id(-1表示没有随机)
    Param_8 = -1,                 -- 随机召唤结束id(-1表示没有随机)
  

}




local sk_zhaohuan = sk_mk{
  

 
    H_1 = h_mk{                  --召唤
        TargetType = 1,
        I_1 = {
            Impact =  i_zhaohuan,
        },
    },
    LogicID= 2,
    LogicParam1= 1,
    LogicParam2= 1, 

  
}

local i_nextattack =i_mk{
    ImpactLogic = 14,        --使用技能时，条件额外使用技能

    Param_1 = 4,             --skilltype，1，普攻，2，任意主动，3指定class，4指定id
    Param_2 = 186101,--"a1",          --技能参数
    Param_3 = 10000,          --概率
    Param_4 = -1,            --额外使用的技能索引（该角色的第几个技能）
    Param_5 = sk_zhaohuan,          --额外使用的技能（skillEx表id，参数4必须填-1才生效）
    Param_6 = 2,             --条件检查时机（1，使用技能前，2，使用技能后）
    Param_7 = 0,             --不能切换目标（0，否（目标死亡后会随机选择目标），1是（目标死亡后追加不能触发））     
    Param_8 = -1,            --条件类型（见17号逻辑）
    Param_9 = -1,            --条件参数1
    Param_10 = -1,           --条件参数2

    Duration = 1,
    AliveCheckType = 1,
}

local sk_main = sk_mk{

    H_1 = h_mk{                 --使用技能触发召唤
        TargetType = 2,
        I_1 = {
            Impact = i_nextattack,
        }
    },
   
}

return sk_main

