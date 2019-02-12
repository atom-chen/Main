--初始化方便调用的全局函数

require('class')
local Battle = require("BattleCore/Battle")
local common = require("common")

local battleTb = {}
local lastBattle = nil

--创建一个新的战斗
function NewBattle(id)
    battleTb[id] = new(Battle)
    battleTb[id].id = id
end

--销毁战斗
function CloseBattle(id)
    battleTb[id] = nil
end

local exportAPI = {
    'Init',
    'BattleStart',
    'WaveStart',
    'AddRole',
    'RoundBegin',
    'Round',
    'LoadWaveFromTab',
    'PullMsg',
    'GetBattleStatus',
    'PushCMD',
    'BattleOver',
    'Prepare',
    'SetBattleInitData',
    'PullSyncMsg',
    'PullSyncMsgFull',
    "PushGM",
    'PushHeroAct',
    'GetBattleResult',
    'GetCmdRecords',
    "SetIsMultiPlayer",
    "ShouldStartRound",
}

for _,api in ipairs(exportAPI) do
    _G[api] = function(id,...)
        local battle = battleTb[id]
        if battle == nil then
             error(string.format("call %s faield,no such battle:",api ,tostring(id)))
             return
        end
        -- local ret,msg = pcall(lastBattle[api],lastBattle,...)
        -- if not ret then
        --     common.warn(msg)
        --     return
        -- end
        return battle[api](battle,...)
    end
end