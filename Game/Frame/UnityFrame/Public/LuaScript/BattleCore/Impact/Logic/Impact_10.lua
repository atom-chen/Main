--随机触发子效果
--参数
--0，随机类型，（0第一次激活随机，之后没吃固定生效,1每次效果生效时随机）
--1，Impact1,
--2，权重1,
--3，Impact2,
--4，权重2，
--。。。

local Impact = require("BattleCore/Impact/Impact")

require("class")
local common = require("common")

local Impact_10 = class("Impact_10",Impact)

function Impact_10:OnActive()
    Impact_10.__base.OnActive(self)
    self.RandType = self.tab.Param[1]
    if self.RandType == 0 then
        self.ChildImpactId = self:RandomImpact()
    end
end

function Impact_10:OnImpact()
    Impact_10.__base.OnImpact(self)

    if not self:CanEffected() then
        return
    end

    if self.RandType == 1 then
        self.ChildImpactId = self:RandomImpact()
    end
    if self.ChildImpactId ~= -1 then
        local impact = Impact.SendImpactToTarget(self.ChildImpactId,self.recver,self.sender)
        if impact ~= nil then
            self:ImpactEffected()                    
        end
    end
end

function Impact_10:RandomImpact()
    local param = self.tab.Param
    local len = #param
    local sum = 0
    local impacts = {}
    for i=2,len-1,2 do
        local impactId,chance = param[i],param[i+1]
        if chance > 0 then
            sum = sum + chance
            table.insert( impacts,{
                impactId,sum
            })
        end
    end
    local rand = self.recver.battle:Random(0,sum)
    for _,t in ipairs(impacts) do
        if t[2] > rand then
            return t[1]
        end
    end
    return -1
end

return Impact_10