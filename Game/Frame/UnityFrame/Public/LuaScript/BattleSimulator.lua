--战斗模拟器
--根据固定的初始化数据、输入，模拟一场战斗
require('class')
local Battle = require("BattleCore/Battle")
local common = require("common")
local lastBattle = nil
local warn = common.mk_warn('BattleSimulator')

local pb = require ('protobuf')


local function Simulate()
    if lastBattle == nil then
        warn("BattleSimulate failed,not Begin yet")        
        return nil
    end

    lastBattle:Prepare()
    lastBattle:BattleStart()
    local winSide = lastBattle:CalcWinSide()

    local iterCount = 0

    if winSide == -1 then
        while (lastBattle.waveIndex < lastBattle.waveMax) do
            
            --每波的循环
            lastBattle:WaveStart()
            while winSide == -1 do
                iterCount = iterCount + 1
                --防止死循环
                if iterCount > 1000 then
                    error('dead loop!')
                    break
                end

                local _,_ = lastBattle:RoundBegin()
                lastBattle:Round()
                if lastBattle:IsWaveClear() then
                    break
                end
                winSide = lastBattle:CalcWinSide()
                if winSide ~= -1 then
                    break
                end
            end

            winSide = lastBattle:CalcWinSide()
            if winSide ~= -1 then
                break
            end
        end
    end

    lastBattle:BattleOver()
    winSide = lastBattle:CalcWinSide()    
    return winSide
end

function BSM_BeginSimulate(buff,len)
    lastBattle = new(Battle)

    local battleSimData = pb.decode('ProtobufPacket.BattleSimulateData',buff,len)    

    lastBattle.isSimulating = true
    lastBattle.cmdsInput = battleSimData.cmds

    lastBattle:Init(battleSimData.battleId,battleSimData.battleSeed)
    lastBattle:_SetBattleInitData(battleSimData.initData)
    Simulate()
end

function BSM_EndSimulate()
    lastBattle = nil
end

function BSM_GetResult()
    if lastBattle == nil then
        warn("BattleSimulate failed,not Begin yet")
        return nil
    end
    return lastBattle:GetBattleResult()
end