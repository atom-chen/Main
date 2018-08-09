--回合开始时，若指定impact存在，则触发技能，技能目标自己
--参数
--1，impact的id
--2, 技能id
--3，立即释放(1,立即；其他顺序)

local Impact = require("BattleCore/Impact/Impact")
local common = require("common")
local warn = common.mk_warn('Impact')

require("class")

local Impact_46 = class("Impact_46",Impact)

function Impact_46:OnActive()
    Impact_46.__base.OnImpact(self)
    local param = self.tab.Param
    self.triggerImpactId = param[1]
    self.skillId = param[2]
    self.castImm = param[3]
end


function Impact_46:msgRoundJustBegin()

    if not self.recver:HasImpact(self.triggerImpactId) then
        return
    end

    if not self:CanEffected() then
        return
    end

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

return Impact_46