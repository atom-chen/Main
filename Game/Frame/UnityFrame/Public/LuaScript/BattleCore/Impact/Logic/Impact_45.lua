--受到特定的impact后，触发使用技能，技能目标Impact 发送者，通过hit做差异
--参数
--1，impact的subclass
--2，impact的tag（-1表示任意）
--3, 技能id
--4，立即释放(1,立即；其他顺序)
--5，概率
--6, 前几次必然触发

local Impact = require("BattleCore/Impact/Impact")
local common = require("common")
local warn = common.mk_warn('Impact')

require("class")

local Impact_45 = class("Impact_45",Impact)

function Impact_45:OnActive()
    Impact_45.__base.OnImpact(self)
    local param = self.tab.Param
    self.triggerSubClass = param[1]
    self.triggerTag = param[2]
    self.skillId = param[3]
    self.castImm = param[4]
    self.chance = param[5]
    self.alwaysCount = param[6]

    self.count = 0
end


function Impact_45:msgRecvImpact(impact)

    if impact.sender == nil then
        return
    end

    if not self:CanEffected() then
        return
    end

    --subclass不满足
    if not impact:IsSubClass(self.triggerSubClass) then
        return
    end

    --tag不满足
    if self.triggerTag ~= -1 and self.triggerTag ~= impact.tag then
        return
    end

    if self.count >= self.alwaysCount then
        if not self.recver.battle:IsRandLt(self.chance) then
            return
        end
    end

    local ret = false
    if self.castImm then
        ret = self.recver:CastSkill(self.skillId,impact.sender.id)
    else
        ret = self.recver:ActUseSkillByID(self.skillId,impact.sender.id)
    end

    if ret then
        self:ImpactEffected()
    end

    self.count = self.count + 1

end

return Impact_45