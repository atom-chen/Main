--驱散目标所有召唤物（注意目标应该是召唤者，而不是召唤物）
--参数

local Impact = require("BattleCore/Impact/Impact")
local common = require("common")
local warn = common.mk_warn('Impact')

require("class")

local Impact_43 = class("Impact_43",Impact)

function Impact_43:OnActive()
    Impact_43.__base.OnActive(self)
    if self.recver ~= nil and self.recver.isValid then
        self.recver:ClearAllSummon()
    end
end

return Impact_43