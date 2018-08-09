--伤害折射
--受到伤害后，会把伤害折射给一个非攻击者玩家
--参数
--表现用的技能id
--概率

local Impact = require("BattleCore/Impact/Impact")
local HitType = require('BattleCore/Common/HitType')
local BattleEventType = require('BattleCore/Common/BattleEventType')
local SkillProcess = require('BattleCore/SkillProcess/SkillProcess')

require("class")
local common = require("common")

local Impact_18 = class("Impact_18",Impact)

function Impact_18:RefixDamage(ret)

    if not self:CanEffected() then
        return
    end

    local impact = ret.impact
    if impact == nil then
        return
    end

    if not impact:IsDirective() then
        return
    end

    local battle = self.recver.battle
    if battle == nil then
        return
    end
    
    local chance = self.tab.Param[2]
    if not battle:IsRandLt(chance) then
        return
    end
    
    local attacker = battle:GetRoleById(ret.senderId)
    if attacker == nil then
        return
    end

    local recver = self.recver

    --是否有一个非攻击者的目标
    --修改成，选择一个敌方目标，并且目标不是攻击者
    local roles = battle:GetRoleEnemies(recver)
    if roles == nil then
        return
    end
    common.removec(roles,function(r) return r == recver or r == attacker or SkillProcess.TargetFilter(r) end)
    local target = battle:RandomSelectOne(roles)
    if target == nil then
        return
    end

    ret.isImmue = true
    --产生一个新的伤害，随机打个某个角色

    local newDmg = {
        senderId = self.recver.id,
        value = ret.value,
    }

    battle:NewEventGroup()
    target:RecvDamage(newDmg)
    local events = battle:PopEventGroup()

    battle:AddEvent({
        type = BattleEventType.UseSkill,
        skillEvent = {
            targetID = target.id,
            usedSkillID = self.tab.Param[1],
            casterID = self.recver.id,
            hits = {
                {
                    hitResults = {
                        {
                            targetID = target.id,
                            events = events,
                        }
                    }
                }
            }
        },
    })
    self:ImpactEffected()    
end

return Impact_18