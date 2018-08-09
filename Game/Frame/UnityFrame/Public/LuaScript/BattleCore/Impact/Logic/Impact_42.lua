--组合技，监听1,2技能使用，通过组合替换3技能
--参数
--1,组合1,1后替换的技能id
--2,组合2,2后替换的技能id
--3,组合1,2后替换的技能id
--4,组合2,1后替换的技能id
--5,是否是baseId，（1，1，2，3，4参数都是baseId，而不是exId）

local Impact = require("BattleCore/Impact/Impact")
local common = require("common")
local warn = common.mk_warn('Impact')

require("class")

local Impact_42 = class("Impact_42",Impact)

function Impact_42:OnActive()
    Impact_42.__base.OnActive(self)
    local param = self.tab.Param
    self.skill11 = param[1]
    self.skill22 = param[2]
    self.skill12 = param[3]
    self.skill21 = param[4]

    self.isBaseId = param[5] == 1

    self.usedSkill = {}
end

function Impact_42:msgAfterUseSkillByIndex( skillIndex )
    if (skillIndex == 3) then
        self.usedSkill = {}
        if self.recver.isValid and self.recver:IsAlive() then
            self.recver:ResumeSkills()
        end
    else
        if #self.usedSkill < 2 then
            table.insert( self.usedSkill,skillIndex )
            if #self.usedSkill >= 2 then
                if self:RefixSkillId() then
                    self:ImpactEffected()
                end
            end
        end
    end
end

function Impact_42:RefixSkillId()

    if #self.usedSkill < 2 then
        return false
    end

    local refixedSkillId = -1
    if self.usedSkill[1] == 1 then
        if self.usedSkill[2] == 1 then
            refixedSkillId = self.skill11
        elseif self.usedSkill[2] == 2 then
            refixedSkillId = self.skill12
        end
    elseif self.usedSkill[1] == 2 then
        if self.usedSkill[2] == 1 then
            refixedSkillId = self.skill21
        elseif self.usedSkill[2] == 2 then
            refixedSkillId = self.skill22
        end
    end

    if refixedSkillId == -1 then
        return false
    end

    if self.recver.isValid and self.recver:IsAlive() then
        if not self.isBaseId then
            self.recver:ReplaceSkills(-1,-1,refixedSkillId,true)
        else
            self.recver:ReplaceSkillsByBase(-1,-1,refixedSkillId,true)
        end
    end

    return true
end

return Impact_42