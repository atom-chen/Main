--击杀敌人后，触发标记buff
--参数：
--1.发送的标记buff

local Impact = require("BattleCore/Impact/Impact")

require("class")
local common = require("common")

local Impact_61 = class("Impact_61",Impact)

function Impact_61:msgKill(target)

    if not self:CanEffected() then
        return
    end

    local tab = self.tab
    local recver = self.recver

    if recver == nil then
        return
    end

    local buffId = tab.Param[1]

    local impact = Impact.SendImpactToTarget(buffId,target,recver,nil,self.skillInfo)
    if impact ~= nil then
        self:ImpactEffected()
    end
end

return Impact_61