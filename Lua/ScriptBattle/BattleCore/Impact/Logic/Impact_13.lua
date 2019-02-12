--死后立即复活
--参数
--0,激活次数
--1,恢复的血量百分比(10000)
--2,复活后，额外放一个技能（当前回合行动）

local Impact = require("BattleCore/Impact/Impact")
local common = require("common")
local warn = common.mk_warn('Impact')

require("class")

local Impact_13 = class("Impact_13",Impact)

function Impact_13:msgBeforeDead(killerId)
    self.count = self.count or self.tab.Param[1]
    if self.count <= 0 then
        return
    end
    if not self:CanEffected() then
        return
    end
    self.count = self.count - 1
    local percent = self.tab.Param[2] / 10000
    --回血
    local hpGain = math.ceil(self.recver:GetMaxHP() * percent)
    self.recver:RecvHeal({
        impact = self,
        value = hpGain,
        senderId = self.sender.id,
        isResurrect = true,
    })
    --放一次技能
    if self.tab.Param[3] ~= -1 then
        self.recver:ActUseSkillByID(self.tab.Param[3],killerId)
    end
    self:ImpactEffected()        
    
end

return Impact_13