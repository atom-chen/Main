--战斗开始（包括新波次）时，第一个行动的符灵伤害翻倍，若第一个行动的符灵的技能不是直接攻击，则这个效果被浪费
--参数：
--伤害倍数


local Impact = require("BattleCore/Impact/Impact")
local common = require("common")
local warn = common.mk_warn('Impact')

require("class")

local Impact_32 = class("Impact_32",Impact)

function Impact_32:bcWaveStart( waveIndex )
    self.willEffect = false
end

function Impact_32:msgRoundBegin()
    local battle = self.recver.battle
    if battle == nil then
        return
    end
    if battle.roundCount ~= 0 then
        return
    end

    self.willEffect = true
end

function Impact_32:RefixSendDamage(ret)
    if not self.willEffect then
        return
    end

    if not self:CanEffected() then
        return
    end

    local val = math.ceil( ret.value * ((10000 + self.tab.Param[1]) / 10000) )
    val = math.max(0,val)
    ret.value = val
    self:ImpactEffected()
end

--只要是用过技能，就废掉
function Impact_32:msgAfterUseSkill(process)
    self.willEffect = false
end

return Impact_32