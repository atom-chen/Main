--特定符灵死亡时,如果自己的血量大于一定值，则分一定百分比的血量给他
--参数
--1,cardId（特定符灵）
--2,大于多少血量百分比会分血(10000)
--3,奉献的剩余血量百分比(10000)
--4,表现用的技能id

local Impact = require("BattleCore/Impact/Impact")
local common = require("common")
local warn = common.mk_warn('Impact')
local HitType = require('BattleCore/Common/HitType')
local BattleEventType = require('BattleCore/Common/BattleEventType')


require("class")

local Impact_20 = class("Impact_20",Impact)

function Impact_20:OnActive()
    Impact_20.__base.OnActive(self)
    self.specCardId = self.tab.Param[1]
end

function Impact_20:bcBeforeDead(role,killerId)

    if not self:CanEffected() then
        return
    end

    local recver = self.recver

    if role:GetHP() > 0 then
         return
    end

    if role.side ~= recver.side then
        return
    end

    if not self:CheckIsSpecCard(role) then
        return
    end

    if not recver:IsAlive() then
        return
    end

    local percent = self.tab.Param[2] / 10000
    if (recver:GetHP() / recver:GetMaxHP()) < percent then
        return
    end

    local sacrificePercent = self.tab.Param[3] / 10000
    --回血
    local hpGain = math.ceil(recver:GetHP() * sacrificePercent)

    local battle = recver.battle

    if battle == nil then
        return
    end

    battle:NewEventGroup()
    --自己扣血
    recver:RecvDamage({
        impact = self,
        value = hpGain,
        senderId = recver.id,
        hitType = HitType.Sacrifice,
    })
    local dmgEvents = battle:PopEventGroup()

    battle:NewEventGroup()
    role:RecvHeal({
        impact = self,
        value = hpGain,
        senderId = self.sender.id,
        isResurrect = true,
    })
    local healEvents = battle:PopEventGroup()
    
    battle:AddEvent({
        type = BattleEventType.UseSkill,
        skillEvent = {
            targetID = role.id,
            usedSkillID = self.tab.Param[4],
            casterID = recver.id,
            hits = {
                {
                    isAnimHit = true,
                    hitResults = {
                        {
                            targetID = role.id,
                            events = healEvents,
                        },
                        {
                            targetID = recver.id,
                            events = dmgEvents,
                        },
                    }
                }
            }
        },
    })
    self:ImpactEffected()    
    
end

return Impact_20