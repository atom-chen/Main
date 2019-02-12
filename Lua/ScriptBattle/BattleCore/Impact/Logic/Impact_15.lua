--下回合立即行动
--参数
--0，是否无视主角（1无视，0不无视）
--1，提示用字典号

local Impact = require("BattleCore/Impact/Impact")

require("class")
local AttrType = require("BattleCore/Common/AttrType")
local common = require("common")
local HitType = require('BattleCore/Common/HitType')

local Impact_15 = class("Impact_15",Impact)

function Impact_15:OnImpact()
    Impact_15.__base.OnImpact(self)

    if not self:CanEffected() then
        return
    end

    local recver = self.recver
    if recver == nil then
        return
    end
    local battle = recver.battle
    if battle == nil then
        return
    end
    local tab = self.tab
    recver:MoveToFirst(tab.Param[1] == 1)
    recver:NotifyHit(HitType.ActGain,tab.Param[2])
    self:ImpactEffected()        
    
end

return Impact_15