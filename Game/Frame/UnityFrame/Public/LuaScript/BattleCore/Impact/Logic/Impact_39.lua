--发送过指定数量的impact后，触发使用技能，技能目标自己，通过hit做差异
--参数
--1，impact的tag（-1表示任意）
--2，数量
--3，技能id
--4，立即释放(1,立即；其他顺序)

local Impact = require("BattleCore/Impact/Impact")
local common = require("common")
local warn = common.mk_warn('Impact')

require("class")

local Impact_39 = class("Impact_39",Impact)

function Impact_39:OnActive()
    Impact_39.__base.OnImpact(self)
    self.triggerTag = self.tab.Param[1]
    self.triggerCount = self.tab.Param[2]
    self.skillId = self.tab.Param[3]
    self.castImm = self.tab.Param[4] == 1
    self.count = 0
end


function Impact_39:msgOnSendImpactEffected(impact)

    if not self:CanEffected() then
        return
    end

    if self.triggerTag == -1 then
        self.count = self.count + 1
    elseif self.triggerTag == impact.tag then
        self.count = self.count + 1
    else
        return
    end

    if self.count >= self.triggerCount then
        self.count = 0
        local ret = false
        if self.castImm then
            ret = self.recver:CastSkill(self.skillId,self.recver.id)
        else
            ret = self.recver:ActUseSkillByID(self.skillId,self.recver.id)
        end
    
        if ret then
            self:ImpactEffected()
        end
    end
end

return Impact_39