--普通伤害，天火伤害，结算伤害后，平摊给给所有人
--参数：
--0，技能系数
--1，技能固定值系数

local Impact = require("BattleCore/Impact/Impact")

require("class")
local AttrType = require("BattleCore/Common/AttrType")
local common = require("common")
local CalcParams = require("BattleCore/CalcParams")
local ImpactUtils = require('BattleCore/Impact/ImpactUtils')
local HitType = require('BattleCore/Common/HitType')


local Impact_60 = class("Impact_60",Impact)

function Impact_60:OnActive()
    Impact_60.__base.OnActive(self)
end

function Impact_60:DoCalc()
    local tab = self.tab
    local sender = self.sender
    local recver = self.recver

    if sender == nil or recver == nil then
        return
    end

    if not sender.isValid or not recver:IsAlive() then
        return
    end

    local power = tab.Param[1]
    local ex = tab.Param[2]

    local baseAttack = sender:GetAttrValue(AttrType.Attack)

    --每步确保都要取整
    local step0 = math.ceil(baseAttack)
    local step1 = math.ceil(step0 * (power / 10000))

    local final = step1 + ex

    final = math.max(0,final)

    return final
end

function Impact_60:OnImpact()
    Impact_60.__base.OnImpact(self)

    if not self:CanEffected() then
        return
    end

    local recver = self.recver

    if recver == nil or not recver:IsAlive() or not recver.isValid then
        return
    end

    local damage = self:DoCalc()

    local allTargets = recver.battle:FindAliveBySide(recver.side)
    local count = #allTargets
    local damagePer = math.floor( damage / count )

    if damagePer <= 0 then
        return
    end

    for _,target in ipairs(allTargets) do
        target:RecvDamage({
            impact = self,
            value = damagePer,
            senderId = self.sender.id,
        })
    end

    self:ImpactEffected()
end

return Impact_60