--伤害溅射，产生伤害后，按照比例溅射给敌方2其他人
--参数
--1，溅射伤害比例
--2，暴击才触发
--3，溅射数量，-1表示所有人

local Impact = require("BattleCore/Impact/Impact")
local common = require("common")
local warn = common.mk_warn('Impact')


require("class")

local Impact_44 = class("Impact_44",Impact)

function Impact_44:OnActive()

    self.damagePercent = self.tab.Param[1]
    self.onlyCrit = self.tab.Param[2] == 1
    self.splashCount = self.tab.Param[3]

end

function Impact_44:msgDamageOther(ret)
    if not self:CanEffected() then
        return
    end

    if ret.isSplash then
        return
    end

    if ret.targetId == nil or ret.targetId == -1 then
        return
    end

    local recver = self.recver

    local target = recver.battle:GetRoleById(ret.targetId)
    if target == nil then
        return
    end

    if target == recver then
        return
    end

    if self.onlyCrit then
        if not ret.isCrit then
            return
        end
    end

    if ret.value == 0 then
        return
    end

    local dmg = math.ceil(ret.value * (self.damagePercent / 10000))

    if dmg <= 0 then
        return
    end

    --获取除过目标外的其他角色
    local targets = target.battle:GetRoleAlies(target)
    common.removec(targets,function(r)
        return not (r:IsAlive() and r ~= target)
    end)

    local validTargets = targets
    if self.splashCount > 0 then
        validTargets = target.battle:RandomSelect(targets,self.splashCount)
    end

    if #validTargets == 0 then
        return
    end

    --触发伤害
    for _,t in ipairs(validTargets) do
        local newRet = {
            impact = self,
            value = dmg,
            senderId = recver.id,
            isSplash = true,
        }
        t:RecvDamage(newRet)
    end

    self:ImpactEffected()
    
end

return Impact_44