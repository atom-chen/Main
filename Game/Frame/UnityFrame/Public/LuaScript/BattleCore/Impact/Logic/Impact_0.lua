--普通伤害
--参数：
--0，技能系数
--1，技能固定值系数
--2，同一技能相同impact多次命中衰减系数

--计算一次，之后每次impact施加相同的效果

local Impact = require("BattleCore/Impact/Impact")

require("class")
local AttrType = require("BattleCore/Common/AttrType")
local common = require("common")
local CalcParams = require("BattleCore/CalcParams")
local ImpactUtils = require('BattleCore/Impact/ImpactUtils')
local HitType = require('BattleCore/Common/HitType')


local Impact_0 = class("Impact_0",Impact)

function Impact_0:OnActive()
    Impact_0.__base.OnActive(self)
    self.damping = self.tab.Param[3]
    self:DoCalc()
end

function Impact_0:DoCalc()
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

    if self.damping > 0 then
        local impactedCount = self:GetImpactedCount(recver.id)
        for i=1,impactedCount do
            power = math.ceil( power * (self.damping / 10000))
        end
    end

    local p = (CalcParams.P2)
    --(攻击 * 300) / (对方防御 + 300）
    local baseAttack = math.ceil( (sender:GetAttrValue(AttrType.Attack) * p) / (recver:GetAttrValue(AttrType.Defense)  + p) )
    
    --初始暴击效果
    local isCrit,critEffect = ImpactUtils.CalcCrit(sender,recver)

    local refix = sender:GetAttrValue(AttrType.DmgEnhance) - recver:GetAttrValue(AttrType.DmgReduce) + 10000
    refix = math.max(CalcParams.P4,refix)

    --每步确保都要取整
    local step0 = math.ceil(baseAttack)
    local step1 = math.ceil(step0 * (refix / 10000))
    local step2 = math.ceil(step1 * (critEffect / 10000))
    local step3 = math.ceil(step2 * (power / 10000))
    local final = step3 + ex

    final = math.max(0,final)

    local ret = {
        impact = self,
        value = final,
        isCrit = isCrit,
        senderId = sender.id,
        refixByEnv = true,
    }

    self.ret = ret
end

function Impact_0:OnImpact()
    Impact_0.__base.OnImpact(self)
    
    if self.ret == nil then
        return
    end

    if not self:CanEffected() then
        return
    end

    local recver = self.recver

    if recver == nil or not recver:IsAlive() or not recver.isValid then
        return
    end

    recver:RecvDamage(self.ret)
    self:ImpactEffected()

    --如果是衰减伤害，在skillInfo上记录一下，是第几次伤害
    if self.damping > 0 then
        self:IncImpactedCount(recver.id)
    end
end

return Impact_0