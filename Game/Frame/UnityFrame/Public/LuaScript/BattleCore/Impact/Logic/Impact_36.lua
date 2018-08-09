--技能替换，激活时，替换指定的技能为新技能
--参数
--1,技能1替换id，不替换配-1
--2,技能2替换id，不替换配-1
--3,技能3替换id，不替换配-1
--4,是否同步到客户端，（1同步，其他不同步）
--5,是否是baseId，（1，1，2，3参数都是baseId而不是exId）

local Impact = require("BattleCore/Impact/Impact")
local common = require("common")
local warn = common.mk_warn('Impact')

require("class")

local Impact_36 = class("Impact_36",Impact)

--修正技能使用
function Impact_36:OnImpactFadeIn()
    local params = self.tab.Param
    if self.recver ~= nil and self.recver.isValid then
        if params[5] ~= 1 then
            self.recver:ReplaceSkills(params[1],params[2],params[3],params[4] == 1)
        else
            self.recver:ReplaceSkillsByBase(params[1],params[2],params[3],params[4] == 1)
        end
    end
end

function Impact_36:OnImpactFadeOut(autoFadeOut)
    if self.recver ~= nil and self.recver.isValid then
        self.recver:ResumeSkills()
    end
end

return Impact_36