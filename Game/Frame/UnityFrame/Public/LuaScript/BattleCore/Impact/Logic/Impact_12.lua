--清CD
--参数
--0，类型(0,技能索引；1，冷却ID)
--1，参数

local Impact = require("BattleCore/Impact/Impact")
local common = require("common")
local warn = common.mk_warn('Impact')

require("class")

local Impact_12 = class("Impact_12",Impact)

function Impact_12:OnActive()
    Impact_12.__base.OnActive(self)
    local type = self.tab.Param[1]
    local arg = self.tab.Param[2]
    local recver = self.recver

    if not self:CanEffected() then
        return
    end

    if type == 0 then
        --找到对应的技能，清掉cd
        recver:ClearCooldownByIndex(arg)
    else
        recver:ClearCooldown(arg)
    end
    self:ImpactEffected()            
end

return Impact_12