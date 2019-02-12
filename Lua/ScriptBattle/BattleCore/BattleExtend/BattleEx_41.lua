--战斗扩展逻辑 竞技场 battleId 41 

require("class")
local common = require("common")
local BattleEx = class("BattleEx_41")

local warn = common.mk_warn('BattleEx_41')
local warnf = common.mk_warnf('BattleEx_41')
local Impact = require("BattleCore/Impact/Impact")

local tabMgr = require("TabManager")

function BattleEx:ctor()
end

function BattleEx:Init(battle)
    self.battle = battle
    
end

function BattleEx:AfterRoundEnd(battle)
    if self.battle == nil then
        return
    end
	if self.battle.arenaLevel == nil then
		return
	end
	if self.battle.arenaLevel <= 0 then
		return
	end
	if self.battle.totalRoundCount == 0 then
		local arenaTab = tabMgr.GetByID("ArenaLevel",self.battle.arenaLevel)
		if arenaTab == nil or arenaTab.ImpactId == -1 then
			return
		end
		local impactTab = tabMgr.GetByID('Impact',arenaTab.ImpactId)
		if impactTab == nil then
			return
		end
		self.arenaTab = arenaTab
		self.impactTab = impactTab
	end
	if self.arenaTab == nil then
		return
	end
	if self.impactTab == nil then
		return
	end
	
	local arenaTab = self.arenaTab
	local impactTab = self.impactTab
	
	--x回合后 每隔y回合加buff
	local curRound = self.battle.totalRoundCount +1
	if curRound < arenaTab.RoundCount then
		return
	end
	
	local stepRound = arenaTab.RoundStep
	if stepRound < 1 then
		stepRound = 1
	end
	if (curRound-arenaTab.RoundCount) % stepRound ~= 0 then
		return
	end
	
	--self.battle:Notice(impactTab.ImpactDesc)
	--全员加buf
	for _,role in ipairs(self.battle.roles) do
		if role.isValid then
		   Impact.SendImpactToTarget(impactTab,role)
		end
	end

end

return BattleEx