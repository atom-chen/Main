--增加妖气
--参数：
--增减量


local Impact = require("BattleCore/Impact/Impact")
local common = require("common")
local warn = common.mk_warn('Impact')

require("class")

local Impact_26 = class("Impact_26",Impact)

--只在激活时生效一次
function Impact_26:OnActive()
    Impact_26.__base.OnActive(self)
    local recver = self.recver
    local tab = self.tab

    if not self:CanEffected() then
        return
    end

    local val = tab.Param[1]

    if recver == nil then
        return
    end

    if recver.battle ~= nil then
        recver.battle:IncSpirit(val)
    end

    self:ImpactEffected()    
end

return Impact_26