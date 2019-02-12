--触发暴击时，根据伤害量吸血
--参数
--1,吸血比例

local Impact = require("BattleCore/Impact/Impact")
local common = require("common")
local warn = common.mk_warn('Impact')


require("class")

local Impact_49 = class("Impact_49",Impact)

function Impact_49:msgDamageOther(ret)
    if not ret.isCrit then
        return
    end

    if not self.recver:IsAlive() or not self.recver.isValid then
        return
    end

    if not self:CanEffected() then
        return
    end

    local percent = self.tab.Param[1]
    local hpGain = math.ceil(ret.value * (percent / 10000))

    self.recver:RecvHeal({
        impact = self,
        value = hpGain,
        senderId = self.recver.id,
    })
    
    if succ then
        self:ImpactEffected()
    end
    
end

return Impact_49