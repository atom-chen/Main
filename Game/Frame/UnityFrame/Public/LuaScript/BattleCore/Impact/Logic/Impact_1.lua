--治疗
--参数：
--治疗类型，(1攻击力，2治疗者血上限，3被治疗者血上限)
--技能系数

local Impact = require("BattleCore/Impact/Impact")
require("class")
local AttrType = require("BattleCore/Common/AttrType")
local common = require("common")
local warn = common.mk_warn('Impact')
local ImpactUtils = require('BattleCore/Impact/ImpactUtils')
local CalcParams = require("BattleCore/CalcParams")


local Impact_1 = class("Impact_1",Impact)


function Impact_1:OnActive()
    Impact_1.__base.OnActive(self)
    self:DoCalc()
end

function Impact_1:DoCalc()
    local sender = self.sender
    local recver = self.recver

    if sender == nil or recver == nil then
        return
    end

    local tab = self.tab
    local healType = tab.Param[1]
    local power = tab.Param[2]

    local step0 = -1
    if healType == 1 then

        --初始暴击效果
        local isCrit,critEffect = ImpactUtils.CalcCrit(sender,recver)
        step0 = math.ceil(sender:GetAttrValue(AttrType.Attack) * (critEffect  / 10000))

    elseif healType == 2 then
        step0 = math.ceil(sender:GetMaxHP())
    else
        step0 = math.ceil(recver:GetMaxHP())
    end

    if step0 < 0 then
        warn("heal error!")
        return
    end

    local enhance = sender:GetAttrValue(AttrType.HealEnhance) + recver:GetAttrValue(AttrType.HealRecvEnhace) + 10000
    enhance = math.max(CalcParams.P5,enhance)

    local step1 = math.ceil(step0 * (enhance / 10000))
    local step2 = math.ceil( step1 * (power / 10000) )

    self.ret = {
        impact = self,
        value = step2,
        senderId = sender.id,
    }
end

function Impact_1:OnImpact()
    Impact_1.__base.OnImpact(self)

    if self.ret == nil then
        return
    end
    if not self:CanEffected() then
        return
    end
    self.recver:RecvHeal(self.ret)
    self:ImpactEffected()
end

return Impact_1