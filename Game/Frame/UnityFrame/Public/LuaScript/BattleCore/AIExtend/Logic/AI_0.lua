--海神特殊逻辑
--使用了2技能后，按照回合，如果特定buff还在，则使用一次大招，否则，使用普攻

local AI = require("BattleCore/AIExtend/AIBase")
local HitType = require('BattleCore/Common/HitType')
local tabMgr = require('TabManager')
local BattleEventType = require('BattleCore/Common/BattleEventType')

require("class")

local AI_0 = class("AI_0",AI)

function AI_0:OnInit()
    if self.role == nil then
        return
    end

    if self.role.aiTab == nil then
        return
    end

    self.buffId = self.role.aiTab.Param[1]
    self.exSkillId = self.role.aiTab.Param[2]
    self.gainRound = self.role.aiTab.Param[3]
    self.gainPower = 0
end

function AI_0:msgAfterUseSkill(skillProcess)
    if skillProcess == nil then
        return
    end
    self.hasBuff = self.role:HasImpact(self.buffId)
    self.gainPower = 0
    self.canCastSpec = nil
    self.broken = nil
end

function AI_0:OnRoleImpactFadeOut(impact,autoFadeOut)
    --特定buff被打掉
    if impact.impactId == self.buffId then
        if not autoFadeOut then
            --打破的表现
            self.battle:AddEvent({
                type = BattleEventType.Hit,
                hitEvent = {
                    targetID = self.role.id,
                    hitType = HitType.Interrupt,
                    val = 115,
                }
            })
            self.gainPower = 0
            self.broken = true
        end
    end
end

function AI_0:msgRoundJustBegin()
    
    if self.role:HasImpact(self.buffId) then
        self.gainPower = self.gainPower + 1
    end
    
    if self.gainPower >= self.gainRound then
        self.canCastSpec = true
    end

end

function AI_0:ProcessAI()
    if self.role == nil or not self.role.isValid or not self.role:IsAlive() then
        return
    end

    --第一回合，直接放大招
    if not self.firstRoundPass then
        self.role:AI_TryCastByIndex(3)
        self.firstRoundPass = true
        return
    end

    if self.role:HasImpact(self.buffId) then
        return
    end
    
    if self.hasBuff then        --是否有大招标记
        if self.canCastSpec then
            --大招
            self.role:AI_TryCast(self.exSkillId)
        else
            --普攻
            self.role:AI_TryCastByIndex(1)
        end
        self.hasBuff = nil
        self.gainPower = 0
        self.canCastSpec = nil
        self.broken = nil
    else
        self.role:ProcessNormalAI()  --走常规AI
    end
end

return AI_0