
--秒杀
--参数：
--1，秒杀类型（0，强制秒杀；1，致死伤害，可能被修正）

local Impact = require("BattleCore/Impact/Impact")

require("class")
local AttrType = require("BattleCore/Common/AttrType")
local common = require("common")

local Impact_3 = class("Impact_3",Impact)

function Impact_3:OnImpact()
    Impact_3.__base.OnImpact(self)

    local type = self.tab.Param[1]
    
    if type == 1 then
        self.recver:SetHP(0)
        self.recver:Die(self.sender.id)                
    else
        self.recver:Dead(self.sender.id) 
    end

    self:ImpactEffected()
end

return Impact_3