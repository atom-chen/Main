--被动效果，特定使用技能时，协同一起使用技能
--参数
--1，cardId（特定符灵,-1时，任意队友使用技能都可协同攻击,-2发送者使用技能）
--2，skillClass（技能类型）
--3，概率
--4，额外使用的技能索引（该角色的第几个技能）
--5，额外使用的技能（skillEx表id，参数3必须填-1才生效）
--6，一回合内激活的次数

local Impact = require("BattleCore/Impact/Impact")
local common = require("common")
local warn = common.mk_warn('Impact')
local SkillProcess = require('BattleCore/SkillProcess/SkillProcess')

require("class")

local Impact_19 = class("Impact_19",Impact)

function Impact_19:OnActive()
    Impact_19.__base.OnActive(self)
    self.count = 0
    self.specCardId = self.tab.Param[1]
end

function Impact_19:bcRoundBegin(caster)
    self.count = 0
end

function Impact_19:CheckValid( caster,skillProcess )
    if skillProcess == nil then
        return false
    end

    local recver = self.recver
    if recver == caster then
        return false
    end

    --当前回合正在行动，不能再协同
    if recver.battle.curRoundRole == recver then
        return false
    end

    if recver.side ~= caster.side then
        return false
    end

    if self.specCardId > 0 and not self:CheckIsSpecCard(caster) then
        return false
    end

    if self.specCardId == -2 then
        if caster ~= self.sender then
            return false
        end
    end

    local skillClass = self.tab.Param[2]
    if skillProcess.tabBase.SkillClass ~= skillClass then
        return false
    end
    local chance = self.tab.Param[3]
    if not self.recver.battle:IsRandLt(chance) then
        return false
    end
    if self.tab.Param[6] > 0 and self.count >= self.tab.Param[6] then
        return false
    end
    --防御性，防止死循环
    if self.count > 1 then
        return false
    end
    local targetId = skillProcess.targetSelectedId
    --是否还活着
    local target = self.recver.battle:GetRoleById(targetId)
    if target == nil or not target:IsAlive() then
        return false
    end

    return true
end

function Impact_19:bcAfterUseSkill(caster, skillProcess )
    if not self:CanEffected() then
        return
    end

    if self.recver.battle.curRoundRole ~= caster then
        return
    end
    
    if not self:CheckValid(caster,skillProcess) then
        return
    end

    local targetId = skillProcess.targetSelectedId
    --是否还活着
    local target = self.recver.battle:GetRoleById(targetId)
    if target == nil or not target:IsAlive() then
        return
    end
    
    self.count = self.count + 1


    local skillIndex = self.tab.Param[4]
    if skillIndex ~= -1 then
        if self.recver:CastSkillByIndex(skillIndex,target.id) then
            self:ImpactEffected()            
        end
        return
    end

    local skillExId = self.tab.Param[5]
    if skillExId ~= -1 then
        if self.recver:CastSkill(skillExId,target.id) then
            self:ImpactEffected()            
        end
        return
    end

end

return Impact_19