--怒气收集机制
require('class')
local common = require("common")

local SPGainer = class("SPGainer")
local HitType = require('BattleCore/Common/HitType')
local BattleEventType = require('BattleCore/Common/BattleEventType')
local BattleCommonData = require("BattleCore/Common/BattleCommonData")

local onBattleStart = 500
local onOurDead = 100
local onEnemyDead = 100
local onOurDamage = 2500

function SPGainer:Init(owner)
    self.owner = owner
    self.battle = owner.battle
    self.sp = 0
    self.allHP = 0
end

function SPGainer:SendMessage(msg,...)
    local func = self[msg]
    if func == nil then
        return
    end
    func(self,...)
end

function SPGainer:bcWaveStart(waveIndex)
    if waveIndex == 1 then
        self:GainSP(onBattleStart)
    end
    self.allHP = 0
    local ours = self.battle:GetRoleAlies(self.owner)
    for _,role in ipairs(ours) do
        if role ~= self.onwer and role.isValid then
            self.allHP  = self.allHP + role:GetMaxHP()
        end
    end
end

function SPGainer:bcRoleDead(roleId,killerId)
    
    if not self.owner:IsAlive() then
        return
    end

    local role = self.battle:GetRoleById(roleId)

    if role == nil then
        return
    end
    
    if role.side == self.owner.side then
        --我方
        self:GainSP(onOurDead)
    else
        --敌方
        self:GainSP(onEnemyDead)
    end
end

function SPGainer:bcRoleDamage(roleId,ret)
    local role = self.battle:GetRoleById(roleId)
    if role == nil then
        return
    end
    --只关心己方受到伤害
    if role.side ~= self.owner.side then
        return
    end
    
    local val = ret.value
    if val == 0 then
        return
    end

    local maxHP = self.allHP
    local gain = math.ceil(val / maxHP * onOurDamage)
    self:GainSP(gain)
end

function SPGainer:GainSP(inc)
    self.sp = self.sp + inc
    self.sp = common.clamp(self.sp,0,BattleCommonData.SP_FULL,self.sp)
    self.battle:AddEvent({
        type = BattleEventType.Hit,
        hitEvent = {
            targetID = self.owner.id,
            hitType = HitType.GainSp,
            val = inc,
        }
    })
end

function SPGainer:CostSP(cost)
    self.sp = self.sp - cost
    self.sp = common.clamp(self.sp,0,BattleCommonData.SP_FULL,self.sp)
    self.battle:AddEvent({
        type = BattleEventType.Hit,
        hitEvent = {
            targetID = self.owner.id,
            hitType = HitType.CostSp,
            val = cost,
        }
    })
end

function SPGainer:SP()
    return self.sp
end

return SPGainer