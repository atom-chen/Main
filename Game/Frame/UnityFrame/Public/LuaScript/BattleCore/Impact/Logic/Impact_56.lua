--被动监听,驱散了指定的buff后，根据驱散量，获得治疗
--参数
--impact class
--治疗基数

local Impact = require("BattleCore/Impact/Impact")
local common = require("common")
local warn = common.mk_warn('Impact')

require("class")

local Impact_56 = class("Impact_56",Impact)

function Impact_56:msgDispellBuffs(impactClass,count)

    if self.tab.Param[1] ~= impactClass then
        return
    end

    if not self:CanEffected() then
        return
    end

    --计算治疗量
    if count <= 0 then
        return
    end

    local p = self.tab.Param[2]
    local fp = p * count

    local val = math.ceil( self.recver:GetMaxHP() * (fp / 10000) )

    local ret = {
        impact = self,
        value = val,
        senderId = self.sender.id,
    }

    self.recver:RecvHeal(ret)

    self:ImpactEffected()
end

return Impact_56