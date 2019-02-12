
require("class")
local common = require("common")
local Random = require("Random")


local Battle = class("Battle")

local SpiritGainPerStep = 10

local Role = require("BattleCore/Role")
local EventPacker = require("BattleCore/EventPacker")
local ActQueue = require("BattleCore/ActQueue")
local BattleEventType = require('BattleCore/Common/BattleEventType')
local BattleSide = require("BattleCore/Common/BattleSide")
local tabMgr = require("TabManager")
local BattleAreaType = require('BattleCore/Common/BattleAreaType')
local AttrType = require('BattleCore/Common/AttrType')
local CmdType = require('BattleCore/Common/CmdType')
local BattleState = require("BattleCore/Common/BattleState")
local BattleActionType = require("BattleCore/ActionType")
local HitType = require('BattleCore/Common/HitType')
local Impact = require("BattleCore/Impact/Impact")
local RoleType = require('BattleCore/Common/RoleType')
local SpawnRule = require("BattleCore/Common/SpawnRule")
local BubbleType = require("BattleCore/Common/BubbleType")

local warn = common.mk_warn('Battle')
local warnf = common.mk_warnf('Battle')


local function findBySide(r,side) return r.side == side end
local function findAliveBySide(r,side) return r:IsAlive() and r.side == side end
local function getByBattlePos(r,battlePosArea,battlePos) return r.battlePosArea == battlePosArea and r.battlePos == battlePos end
local function getByCardId(r,cardId) return r.cardId ~= nil and r.cardId == cardId end
local function getByMonsterId(r,monsterId) return r.monsterId ~= nil and r.monsterId == monsterId end
local function getHero(r,side) return r.side == side and r.isHeroCard end
local function findAliveByUserObjId(r,userObjId) return r:IsAlive() and r.userObjId == userObjId end

function Battle:CalcEventTime(e)
    if e == nil then
        return 0,0
    end

    local time = 0
    local hasPause = 0
    if e.type == BattleEventType.UseSkill then
        --先计算基本时间
        local skillEvent = e.skillEvent
        if skillEvent ~= nil then
            local tab = tabMgr.GetByID("SkillEx",skillEvent.usedSkillID)
            if tab ~= nil then
                time = time + tab.Time
            end
            --递归计算触发的其他事件需要的时间
            if skillEvent.hits ~= nil then
                for _,hit in ipairs(skillEvent.hits) do
                    if hit.hitResults ~= nil then
                        for _,hitResult in ipairs(hit.hitResults) do
                            local t,p = self:CalcEventListTime(hitResult.events)
                            time = time + t
                            hasPause = hasPause + p
                        end
                    end
                end
            end
        end
    elseif e.type == BattleEventType.ChangeEnv then
        time = time + 1000
    elseif e.type == BattleEventType.BattleStart then
        time = time + self.tab.BattleStartTime
    elseif e.type == BattleEventType.WaveStart then
        if e.waveStartEvent.waveIndex > 1 then
            time = time + 2000
        end
    elseif e.type == BattleEventType.Pause then
        hasPause = hasPause + 1
    elseif e.type == BattleEventType.Idle then
        time = time + math.ceil(e.idleEvent.time * 1000)
    elseif e.type == BattleEventType.PlayCutscene then
        -- local tab = tabMgr.GetByID('BattleCutscene',e.playCutsceneEvent.id)
        -- if tab ~= nil then
        --     time = time + tab.Time
        --     if tab.IsPuase == 1 then
        --         hasPause = hasPause + 1
        --     end
        -- end
        hasPause = hasPause + 1
    elseif e.type == BattleEventType.Scenario then
        local t,p = self:CalcEventListTime(e.scenarioEvent.events)
        time = time + t
        hasPause = hasPause + p
    elseif e.type == BattleEventType.PlayStoryContentEvent then
        hasPause = hasPause + 1
    elseif e.type == BattleEventType.Prepare then
        local t,p = self:CalcEventListTime(e.prepareEvent.events)
        time = time + t
        hasPause = hasPause + p
    elseif e.type == BattleEventType.RoundBegin then
        -- if self.shouldStartRound then
        --     time = time + 100
        -- end
    elseif e.type == BattleEventType.PauseEx then
        hasPause = hasPause + 1
    elseif e.type == BattleEventType.ParallelEvent then
        --计算最长的时间
        local tMax = 0
        local pCount = 0
        for _,child in ipairs(e.parallelEvent.events) do
            local t,p = self:CalcEventTime(child)
            if t > tMax then
                tMax = t
            end
            pCount = pCount + p
        end
        --一个平行事件，算一个暂停
        if pCount > 0 then
            hasPause = hasPause + 1
        end
        time = time + tMax
    end

    return time,hasPause
end

 function Battle:CalcEventListTime(events)
    if events == nil then
        return 0,0
    end

    local time,hasPause = 0,0
    for _,e in ipairs(events) do
        local t,p = self:CalcEventTime(e)
        time = time + t
        hasPause = hasPause + p
    end
    return time,hasPause
end

function Battle:Init(battleTabId,seed)
    self.state = BattleState.Invalid
    self.eventPacker = new(EventPacker)
    self.roles = {}
    self.actQ = new(ActQueue)
    self.battleId = battleTabId
    self.tab = tabMgr.GetByID('Battle',battleTabId)
    if self.tab == nil then
        error("no such battle:" .. battleTabId);
    end
    self.idBase = 1
    self.actions = {}
    self.cmds = {}
    self:NewEventGroup()
    self.seed = seed
    self.waveIndex = 0
    self.collectedPreviewDropIds = {}
    self.tick = 0

    self.blueStatist = {}
    self.redStatist = {}

    local scenarioId = self.tab.ScenarioScript
    if scenarioId ~= -1 then
        local Scenario = require('Scenario/Scenario')
        local s = new(Scenario)
        s:Init(self,"Scripts/S_" .. scenarioId)
        self:RegisterListener(s)
    end
	if battleTabId == 41 then
		local BattleEx = require('BattleCore/BattleExtend/BattleEx_41')
		if BattleEx ~= nil then
			 local battleEx = new(BattleEx)
			 battleEx:Init(self)
			 self:RegisterListener(battleEx)
		end
	end
    self.rand = new(Random)
    self.rand:Init(seed)

    self:InitEnv()
end

--随机

function Battle:Random(min,max)
    return self.rand:Rand( min,max)
end

function Battle:Shuffle(t)
    common.shuffle(t,function(min,max)
        return self:Random(min,max)
    end)
end

function Battle:Random10000()
    
    if self.gm_Chance10000 then
        return 0
    end

    return self:Random(0,10000)
end

function Battle:IsRandLt(chacne)
    return chacne >= self:Random10000()
end

function Battle:RandomSelect(list,count)
    if #list <= count then
        return list
    end
    
    local ret = {}
    local len = #list
    for i=1,count do
        local index = self:Random(i,len)
        table.insert(ret,list[index])
        local tmp = list[index]
        list[index] = list[i]
        list[i] = tmp
    end
    return ret
end

function Battle:RandomSelectOne(list)
    local len = #list
    if len == 1 then
        return list[1]
    elseif len == 0 then
        return nil
    end
    return list[self:Random(1,len)]
end

--{{chance,item},{chance,item}}
function Battle:RandomSelectOne_Chnace(list)
    local len = #list
    if len == 1 then
        return list[1][2]
    elseif len == 0 then
        return nil
    end

    local chanceAll = 0
    for _,v in ipairs(list) do
        chanceAll = chanceAll + v[1]
    end
    local rand = self:Random(0,chanceAll)

    local counter = 0
    for _,v in ipairs(list) do
        counter = counter + v[1]
        if counter >= rand then
            return v[2]
        end
    end
    return nil
end
---------------------------------------------

--事件打包相关
function Battle:NewEventGroup()
    self.eventPacker:NewGroup()
end

function Battle:PopEventGroup()
    return self.eventPacker:Pop()
end

function Battle:AddEvent(e)
    --优化，接连两个同类型的事件，按照规则合并
    local lastEvent = self:PeekLastEvent()
    if lastEvent ~= nil and lastEvent.type == e.type then
        if e.type == BattleEventType.ImpactsChange then
            -- --id必须一致
            -- if e.impactsChangeEvent.ownerID == lastEvent.impactsChangeEvent.ownerID then
            --     lastEvent = e   --直接覆盖
            --     return
            -- end
        elseif e.type == BattleEventType.Idle then
            --时间增加
            lastEvent.idleEvent.time = lastEvent.idleEvent.time + e.idleEvent.time
            return
        elseif e.type == BattleEventType.ChangeSpirit then
            lastEvent = e   --直接覆盖
            return
        end
    end
    self.eventPacker:AddEvent(e)
end

function Battle:PeekLastEvent()
    local events = self.eventPacker:Peek()
    if events == nil then
        return nil
    end
    if #events == 0 then
        return nil
    end
    return events[#events]
end

--------------------------------------

--role管理，各种查找函数

--monsterId
--config {side,battlePos,battlePosArea,spawnRule}
function Battle:CreateRoleFromTab(monsterId,isPlaySpawnAnim,config)
    local initData = self:LoadRoleFromTab(monsterId,config.side,config.battlePos,config.battlePosArea)
    if initData == nil then
        return nil
    end
    initData.spawnRule = config.spawnRule or initData.spawnRule
    return self:CreateRole(initData,isPlaySpawnAnim)
end

function Battle:CreateRole(initData,isPlaySpawnAnim)
    if self.state == BattleState.Prepare then
        return self:_CreateRoleAtPrepare(initData,isPlaySpawnAnim)
    else
        return self:_CreateRoleAfterStart(initData,isPlaySpawnAnim)
    end
end

function Battle:_CreateRoleAtPrepare(initData,isPlaySpawnAnim)

    local orgRole = self:GetRole(getByBattlePos,initData.battlePosArea,initData.battlePos)
    if orgRole ~= nil then
        --如果原位置已经有人了
        --规则1，替换掉，直接删除原角色
        --规则2，什么都不做，同位置刷出
        --规则0，创建失败
        if initData.spawnRule == SpawnRule.Replace then
            --直接删除
            self:_DelRoleDirect(orgRole,true)
        elseif initData.spawnRule == SpawnRule.Overlay then
            --do nothing
        elseif initData.spawnRule == SpawnRule.FailedIfHold then
            return nil
		elseif initData.spawnRule == SpawnRule.Update then
			orgRole:Init(initData)
			return orgRole
        else
            return nil
        end
    end

    --保证id稳定
    local id = nil

    if initData.queryId ~= nil and initData.queryId ~= 0 then
        id = initData.queryId
    else
        id = (initData.battlePosArea+1) * 1000 + initData.battlePos * 100 + (initData.userObjId or 0)
    end

    local role = self:_CreateRole(initData,id)
    --不使用被动，不添加buff，不进入移动队列

    isPlaySpawnAnim = isPlaySpawnAnim or initData.spawnRule ~= SpawnRule.FailedIfHold
    self:NotifyRoleCreate(role,isPlaySpawnAnim)

    return role    
end

function Battle:_CreateRoleAfterStart(initData,isPlaySpawnAnim)
    --查看是否位置上已经有人了
    local orgRole = self:GetRole(getByBattlePos,initData.battlePosArea,initData.battlePos)
    if orgRole ~= nil then
        if initData.spawnRule == SpawnRule.Replace then
            self:DelRole(orgRole,true)
        elseif initData.spawnRule == SpawnRule.Overlay then
            --do nothing
        elseif initData.spawnRule == SpawnRule.FailedIfHold then
            --warnf("battlePos[%s:%s] already has a role[%s].add failed",tostring(initData.battlePosArea),tostring(initData.battlePos),tostring(orgRole.id))
            return nil
        else
            return nil
        end
    end

    local id = self.idBase + 1
    self.idBase = id
    local role = self:_CreateRole(initData,id)

    --使用被动，上buff，加入行动队列
    self:_OnRoleCreate(role)

    isPlaySpawnAnim = isPlaySpawnAnim or initData.spawnRule ~= SpawnRule.FailedIfHold
    self:NotifyRoleCreate(role,isPlaySpawnAnim)

    return role
end

function Battle:_CreateRole(initData,id)
    
    local role = new(Role)
    role.battle = self
    role:Init(initData)
    role.id = id

    table.insert( self.roles, role)

    return role
end

function Battle:NotifyRoleCreate(role,isPlaySpawnAnim)
    --先添加
    self:AddEvent({
        type = BattleEventType.AddRole,
        addRoleEvent = {
            --roleID = idBase,
            sync = role:GetSync(),
            isPlaySpawnAnim = isPlaySpawnAnim,
        }
    })
end

function Battle:_OnRoleCreate(role)
    
    if role == nil then
        return
    end

    role:TryCastPassive()
        
    --添加初始buff
    local impacts = nil
    if role.side == BattleSide.bs_Blue then
        impacts = self.blueImpacts
    elseif role.side == BattleSide.bs_Red then
        impacts = self.redImpacts
    end

    if impacts ~= nil then
        for _,impactId in ipairs(impacts) do
            --print(role.id,impactId)
            Impact.SendImpactToTarget(impactId,role,nil)
        end
    end

    --buff可能会改血上限，这里重新初始化血量
    if not role.hasStatus then
        --直接赋值，不走sethp
        role.hp = role:GetMaxHP()
    end

    --初始的血量上限
    role.orgMaxHP = role:GetMaxHP()

    if not role.isHeroCard then
        self.actQ:Enqueue(role)
    end

    self:BroadCastMsg("bcRoleCreate",role)

    --这里是这个角色真正上场
    self:StatistRole(role)
end

function Battle:DelRole(role,isForReplace)
    if role == nil then
        return
    end
    if not role.isValid then
        return
    end
    role:OnDel()
    --单纯标脏处理，防止循环删除迭代器失效
    role.isValid = false
    self:AddEvent({
        type = BattleEventType.DeleteRole,
        deleteRoleEvent = {
            roleID = role.id,
            isForReplace = isForReplace,
        }
    })
end

--危险函数，直接删除，不要随意使用，不要在loop中使用
function Battle:_DelRoleDirect(role,isForReplace)
    self:DelRole(role,isForReplace)
    self:CheckAlive()    
end

function Battle:GetRole(predic,...)
    local roles = self.roles
    for _,role in ipairs(roles) do
        if role.isValid and predic(role,...) then
            return role
        end
    end
    return nil
end

function Battle:GetHero(side)
    return self:GetRole(getHero,side)
end

function Battle:GetRoleByCardId(cardId)
    return self:GetRole(getByCardId,cardId)
end

function Battle:GetRoleByMonsterId(monsterId)
    return self:GetRole(getByMonsterId,monsterId)
end

function Battle:GetRoleById(id)
    local roles = self.roles
    for _,role in ipairs(roles) do
        if role.isValid and role.id == id then
            return role
        end
    end
    return nil
end

function Battle:FindRoles(predic,...)
    local rets = {}
    local roles = self.roles
    for _,r in ipairs(roles) do
        if not r.isHeroCard and r.isValid and predic(r,...) then
            table.insert(rets,r)
        end
    end
    return rets
end

function Battle:FindAliveBySide(side)
    return self:FindRoles(findAliveBySide,side)
end

function Battle:FindRolesIncludeHero(predic,...)
    local rets = {}
    local roles = self.roles
    for _,r in ipairs(roles) do
        if r.isValid and predic(r,...) then
            table.insert(rets,r)
        end
    end
    return rets
end

function Battle:CountRoles(predic,...)
    local count = 0
    local roles = self.roles
    for _,r in ipairs(roles) do
        if not r.isHeroCard and r.isValid and predic(r,...) then
            count = count + 1
        end
    end
    return count
end

function Battle:GetRoleAlies(role)
    return self:FindRoles(findBySide,role.side)
end

function Battle:GetRoleEnemies(role)
    return self:FindRoles(findBySide,role:GetOppositeSide())
end

function Battle:CountEnvRole(envType,side)
    return self:CountRoles(function(r)
        if not r:IsAlive() then
            return false
        end

        if side ~= nil and r.side ~= side then
            return false
        end

        return r.envType == envType
    end)
end

------------------------------------------

--回合操作

--死亡结算
function Battle:CheckAlive()
    common.removec(self.roles,function(r)
        return not r:IsAlive() or not r.isValid
    end)
end

function Battle:Prepare()
    local succ,msg = pcall(Battle._Prepare,self)
    if not succ then
        warn(msg)
    end
end

function Battle:BattleStart()
    local succ,msg = pcall(Battle._BattleStart,self)
    if not succ then
        warn(msg)
    end
end

function Battle:BattleOver()
    self.state = BattleState.BattleEnd
    --self:Sync()
end

function Battle:WaveStart()
    local succ,msg = pcall(Battle._WaveStart,self)
    if not succ then
        warn(msg)
    end
end

function Battle:RoundBegin()
    local succ,ret1,ret2 = pcall(Battle._RoundBegin,self)
    if not succ then
        warn(ret1)
        return false,0
    end
    return ret1,ret2
end

function Battle:Round()
    local ret,msg = pcall(Battle._Round,self)
    if not ret then
        warn(msg)
    end
end


function Battle:_Prepare()
    self.tick = self.tick + 1
    self.waveIndex = 0
    self.state = BattleState.Prepare
    self.eventPacker = new(EventPacker)
    self:NewEventGroup()

    --策划需求，准备阶段先要刷出双方
    self:NewEventGroup()

    --是否有波前动画
    local waveTab = self:GetWaveTab(1)
    if waveTab ~= nil then
        if waveTab.BegCutscene ~= -1 then
            self:PlayCutscene(waveTab.BegCutscene)
        end
    end

    --创建我方蓝方
    self:LoadWave(1,BattleSide.bs_Blue)

    --创建红方
    self:LoadWave(1,BattleSide.bs_Red)

    -- local events = self:PopEventGroup()
    -- common.removec(events,function(e) end)
    -- self:NewEventGroup()

    local blues = self:FindRoles(findBySide,BattleSide.bs_Blue)
    table.insert( blues, self:GetHero(BattleSide.bs_Blue) )
    local reds = self:FindRoles(findBySide,BattleSide.bs_Red)
    table.insert( reds, self:GetHero(BattleSide.bs_Red) )

    local blueSyncs = {}
    for _,role in ipairs(blues) do
        table.insert( blueSyncs, role:GetSync())
    end
    local redSyncs = {}
    for _,role in ipairs(reds) do
        table.insert(redSyncs,role:GetSync())
    end

    local events = self:PopEventGroup()

    --同步一下环境
    self:AddEvent({
        type = BattleEventType.SyncState,
        syncEvent = {
                battleSync = {
                spirit = self.spirit,
                spiritMax = self.spiritMax,
                envID = self:GetCurEnvId(),
            },
        }
    })

    self:AddEvent({
        type = BattleEventType.Prepare,
        prepareEvent = {
            blues = blueSyncs,
            reds = redSyncs,
            events = events,
        }
    })
end


function Battle:_BattleStart()
    --准备阶段也会发生随机，忽略掉准备阶段随机的影响
    self.rand = new(Random)
    self.rand:Init(self.seed)

    self.eventPacker = new(EventPacker)
    self.state = BattleState.BattleStart

    self.totalRoundCount = 0
    
    self.blueStatist = {}
    self.redStatist = {}

    self:NewEventGroup()


    self.waveMax = 0
    local tab = tabMgr.GetByID('Battle',self.battleId)
    if tab ~= nil then      
        for _,waveId in ipairs(tab.Wave) do
            if waveId > 0 then
                self.waveMax = self.waveMax + 1
            end
        end
    end

    --战斗开始，准备阶段的创建的角色，执行角色初始行为
    table.sort(self.roles,function(r1,r2) return r1.id > r2.id end)
    for _,role in ipairs(self.roles) do
        if role.isValid then
            self:_OnRoleCreate(role)
        end
    end

    --初始化环境
    local c0,c1
    if self.hasDefEnv then
        self:ChangeEnvType(self.curEnvType)
    else
        c0,c1 = self:CalcEnv()
        self:ChangeEnvType(self.curEnvType,true)
    end
    -- self.isPVE = true
    -- local reds = self:FindRoles(findBySide,BattleSide.bs_Red)
    -- for _,r in ipairs(reds) do
    --     if r.userObjId ~= nil and r.userObjId ~= -1 then
    --         self.isPVE = false
    --         break
    --     end
    -- end

    if self.enableBubble then
        --开场喊话
        local bubbles ={}
        self:GlobalBubble(BattleSide.bs_Blue,BubbleType.BattleStart,bubbles)
        -- if self.isPVE then
        --     self:PVE_MonsterGlobalBubble(BubbleType.BattleStart,bubbles)
        -- else
        --     self:GlobalBubble(BattleSide.bs_Red,BubbleType.BattleStart,bubbles)
        -- end
        
        --新规则，不再区分pve，还是pvp，统一一个喊话规则
        self:GlobalBubble(BattleSide.bs_Red,BubbleType.BattleStart,bubbles)


        if #bubbles > 0 then
            self:AddEvent({
                type = BattleEventType.Bubble,
                bubbleEvent = {bubbles = bubbles}
            })
        end
    end

    self:AddEvent({
        type = BattleEventType.BattleStart,

        battleStartEvent = {
            envID = self:GetCurEnvId(),
            hasDefaultEnv = self.hasDefEnv,
            count0 = c0,
            count1 = c1,
        }
    })

    --self:self:Sync()
end

function Battle:GlobalBubble(side,bubbleType,bubbles)

    --策划又不要情缘喊话了

    -- local item = self:TryBubbleByTalkAndRespone(side)
    -- if item ~= nil then
    --     --触发了情缘喊话，返回
    --     table.insert(bubbles,item)
    --     return
    -- end

    if not self:IsRandLt(2000) then
        return
    end

    local roles = self:FindRoles(findBySide,side)
    --随机人数
    local count = self:Random(1,2)
    roles = self:RandomSelect(roles,count)
    if roles ~= nil then
        for _,role in ipairs(roles) do
            local item = role:TryBubble(bubbleType)
            if item ~= nil then
                table.insert(bubbles,item)
            end
        end
    end
end

function Battle:TryBubbleByTalkAndRespone(side)
    local roles = self:FindRoles(findBySide,side)
    local selectedRoles = {}
    local findRole = function(r)

        if r.cardId == nil or r.cardId == -1 then
            return
        end

        for _,o in ipairs(selectedRoles) do
            if o == r then
                return
            end
        end

        local tab = tabMgr.GetByID("BattleBubbleCard",r.cardId)
        if tab == nil then
            return
        end

        local findOne = false
        for _,otherCardId in ipairs(tab.OtherCardId) do
            if otherCardId ~= -1 then
                for _,other in ipairs(roles) do
                    if other ~= r and other.cardId == otherCardId then
                        table.insert(selectedRoles,other)
                        findOne = true
                        break
                    end
                end
            end
        end
        if findOne then
            table.insert(selectedRoles,r)
        end
    end

    for _,r in ipairs(roles) do
        findRole(r)
    end

    if #selectedRoles == 0 then
        return nil
    end

    local role = self:RandomSelectOne(selectedRoles)
    if role == nil then
        return nil
    end
    return role:TryBubble(BubbleType.Talk)
end

function Battle:PVE_MonsterGlobalBubble(bubbleType,bubbles)
    local item = self:TryBubbleByTalkAndRespone(BattleSide.bs_Red)
    if item ~= nil then
        table.insert(bubbles,item)
    end
end

function Battle:Bubble(item)

    if not self.enableBubble then
        return
    end

    --检查是否已经有喊话了，策划要求一次只能有一个喊话
    local events = self.eventPacker:Peek()
    if events ~= nil then
        for _,e in ipairs(events) do
            if e.type == BattleEventType.Bubble then
                return
            end
        end
    end

    self:AddEvent({
        type = BattleEventType.Bubble,
        bubbleEvent = {
            bubbles = {item}
        }
    })
end

function Battle:_WaveStart()
    self.lastRoundRole = nil
    self.curRoundRole = nil
    self.roundCount = 0
    self.waveIndex = self.waveIndex + 1
    self.tick = self.tick + 1
    self.state = BattleState.WaveStart

    self:AddEvent({
        type = BattleEventType.WaveStart,
        waveStartEvent = {
            waveIndex = self.waveIndex,
        }
    })

    --print("wave start",self.waveIndex)

    local waveTab = self:GetWaveTab(self.waveIndex)
    --第一波不加载
    if self.waveIndex > 1 then
        --是否播放动画
        if waveTab ~= nil then
            if waveTab.BegCutscene ~= -1 then
                self:PlayCutscene(waveTab.BegCutscene)
            end
        end

        --新的波次开始
        for _,role in ipairs(self.roles) do
            role:NewWaveStart(waveTab,self.waveIndex)
        end

        --加载一波怪
        self:LoadWave(self.waveIndex,BattleSide.bs_Red)
    end

    --没有生命的立即死亡
    for _,role in ipairs(self.roles) do
        if role.isValid and not role:IsAlive() then
            role:Die(-1)
        end
    end

    self:CheckAlive()
    --清空行动队列
    self.actQ:Clear()
    --重新加入
    for _,role in ipairs(self.roles) do
        if role.isValid and not role.isHeroCard then
            self.actQ:Enqueue(role)
        end
    end

    --清空CMD
    --self.cmds = {}

    --统计一下存活的角色，记录一下他们的站位，作为召唤的坑
    self.waveValidPos = {}
    for _,role in ipairs(self.roles) do
        if role.isValid then
            self.waveValidPos[role.battlePosArea] = self.waveValidPos[role.battlePosArea] or {}
            self.waveValidPos[role.battlePosArea][role.battlePos] = true
        end
    end
    --广播
    self:BroadCastMsg("bcWaveStart",self.waveIndex)

    self:Sync()
end

function Battle:_RoundBegin()
    --处理一次CMD，可能有插队列的CMD
    self:ProcessCMD()
    self.tick = self.tick + 1
    --print('round begin')
    self.state = BattleState.RoundStart

    --涨AP阶段
    for _,role in ipairs(self.roles) do
        if role.isValid then
            role:ReCalcSpeed()
        end
    end

    local count = 0
    while not self:RoundAPIter() do
        count = count + 1
        if count >= 1000 then
            warn('round ap failed!')
            break
        end
    end

    self:SyncAP()

    self.state = BattleState.RoundBegin
    local actQ = self.actQ
    if actQ:IsEmpty() then
        warn("empty act q!")
        return false,-1
    end

    while (not actQ:IsEmpty()) do
        local role = actQ:Dequeue()
        if role ~= nil and role:IsAlive() then
            self.curRoundRole = role
            break
        end
    end

    if self.curRoundRole == nil then
        warn("no valid role for act")
        return -1,-1
    end

    --通知一下
    self:SendMessage("BeforeRoundBegin")

    local needPlayerInput = self.curRoundRole:RoundBegin()
    needPlayerInput = needPlayerInput and not self.shouldStartRound
    self.curRoundNeedPlayerInput = needPlayerInput

    self:AddEvent({
        type = BattleEventType.RoundBegin,
        roundBeginEvent = {
            roundRoleID = self.curRoundRole.id,
            isNeedPlayerInput = needPlayerInput,
            roundCount = self.roundCount,
            userObjId = self.curRoundRole.userObjId,
        }
    })

    self:BroadCastMsg('bcRoundBegin',self.curRoundRole)

    self.state = BattleState.Round0

    if not needPlayerInput then
        self:SetRoundStart()
    end

    if self.statistRoles ~= nil then
        local roleTb = self.statistRoles[self.curRoundRole.id]
        if roleTb ~= nil then
            roleTb.actCount = roleTb.actCount + 1
        end
    end
    
    return self.curRoundRole.userObjId or -1,self.curRoundRole.id
end

function Battle:SyncAP()
    local apChanges = {}
    for _,role in ipairs(self.roles) do
        if role.isValid then
            table.insert( apChanges,{
                roleID = role.id,
                newAP = role.ap,
                newActIndex = role:GetActIndex(),
                isWaitingAct = role:IsWaitingAct()
            })
        end
    end
    self:AddEvent({
        type = BattleEventType.RoundAP,
        roundAPEvent = {
            count = count,
            apChanges = apChanges,
        }
    })
end

function Battle:RoundAPIter()
    local actQ = self.actQ
    if actQ:IsEmpty() then
        return true
    end
    --整理
    while not actQ:IsEmpty() do
        local role = actQ:Peek()
        if role == nil or not role:IsAlive() then
            actQ:Dequeue()
            actQ:ReSort()
        else
            --可以行动
            if role:IsWaitingAct() then
                return true
            else
                break
            end
        end
    end
    for _,role in ipairs(self.roles) do
        if role.isValid and not role.isHeroCard then
            role:IncAP()
        end
    end
    actQ:ReSort()
    return false
end

function Battle:RoleApChange(role)
	self.actQ:ReSort()
	--其他角色顺序可能也变了 都更新下 （否则actIdx在表现层出现一样的值，行动条层级出错）
	self:SyncAP()
end

function Battle:_Round()
    --roundBegin后，插入了特殊的指令

    --健壮性保证，一回合内不能触发太多次技能，防止死循环
    for _,r in ipairs(self.roles) do
        r.castCount = 0
    end


    self:ProcessCMD()
    self.tick = self.tick + 1
    self.shouldStartRound = false
    
    if self.curRoundRole == nil then
        self:CheckAlive()
        self:RoundEnd()
        return
    end

    self.state = BattleState.Round
    self:AddEvent({
        type = BattleEventType.Round,
        roundEvent = {
            roundRoleID = self.curRoundRole.id,
            roundCount = self.roundCount,
			totalRoundCount = self.totalRoundCount + 1,
        }
    })
    self:SendMessage("OnRound")
    self:ProcessCMD()
    self.tick = self.tick + 1

    -- if not hasCmd and self.curRoundNeedPlayerInput then
    --     --需要输入，但是没有处理到输入，则AI处理
    --     self.curRoundRole:ProcessAI()
    -- end

    local hasAction = self:ExcuteActions()
    --回合结束前
    self:BeforeRoundEnd()
    --执行回合结束前的Action
    hasAction = self:ExcuteActions() or hasAction

    --新版本，主角AI只会发生在回合开始

    --处理双方主角AI
    -- local heroB = self:GetHero(BattleSide.bs_Blue)
    -- local heroR = self:GetHero(BattleSide.bs_Red)
    -- if heroB ~= nil and (heroB.isAI or heroB.isAuto) then
    --     heroB:ProcessAI()
    -- end
    -- if heroR ~= nil and (heroR.isAI or heroR.isAuto) then
    --     heroR:ProcessAI()
    -- end

    --没有任何动作，被控制了，1s等待
    if not hasAction then
        self:AddEvent({
            type = BattleEventType.Idle,
            idleEvent = {
                time = 0.8,
            }
        })
    end

    --回合真正结束
    self:RoundEnd()

    self:CheckAlive() --整理一下存活的角色

    --self:self:Sync()
    --回合结束
    if self:IsWaveClear() then
        self:AddEvent({
            type = BattleEventType.WaveEnd,
        })
        
        local waveTab = self:GetWaveTab(self.waveIndex)
        if waveTab ~= nil then
            self:PlayCutscene(waveTab.EndCutscene)
        end

    end
    self:ProcessCMD()
    self.tick = self.tick + 1
end

function Battle:BeforeRoundEnd()
    local curRoundRole = self.curRoundRole
    if curRoundRole ~= nil then
        curRoundRole:BeforeRoundEnd()
    end
end

function Battle:RoundEnd()
    --print('round end')
    self.tick = self.tick + 1
    self.state = BattleState.RoundEnd

    --妖气收集改为回合结束才增长
    --使用技能，增加妖气
    self:IncSpirit(SpiritGainPerStep)
    
    local curRoundRole = self.curRoundRole
    curRoundRole:RoundEnd()
    if curRoundRole:IsAlive() then
        if curRoundRole.isHeroCard then
            curRoundRole.actIndex = -1
        else
            self.actQ:Enqueue(curRoundRole)
        end
        --self:SyncAP()
    end
    self.lastRoundRole = curRoundRole
    self.curRoundRole = nil
    self.actedHero = nil

    self:SendMessage("AfterRoundEnd")
    self:BroadCastMsg("bcRoundEnd",curRoundRole)

    if self.gm_HPAutoGain then
        self:Sync()
    end
	
	self:AddEvent({
        type = BattleEventType.RoundEnd,
        roundEndEvent = {
            roundCount = self.totalRoundCount + 1,
        }
    })
	
    self.roundCount = self.roundCount + 1
    self.totalRoundCount = self.totalRoundCount + 1
	--是否有回合次数限制
	if self.tab.RoundCountMax >0 and self.totalRoundCount >= self.tab.RoundCountMax then
		self.forceWinSide = BattleSide.bs_Blue
	end
	
end

--CMD----
local cmdFuncTb = nil
local cmdExeRet = {
    succ = 1,
    failed = 2,
    notnow = 3,
}

function Battle:ProcessCMD()
    local cmds = self.cmds

    --模拟模式，有cmd输入
    if self.isSimulating then
        cmds = {}
        for _,v in ipairs(self.cmdsInput) do
            if v.tick == self.tick and v.state == self.state then
                table.insert(cmds,v.cmd)
            end
            
            if v.tick > self.tick then
                break
            end
        end
    end

    local swap = nil

    if #cmds <= 0 then
        return false
    end

    while #cmds > 0 do
        local cmd = table.remove(cmds,1)
        local ret,exeRet = self:_ProcessCMD(cmd)
        if ret then
            if exeRet == cmdExeRet.notnow then
                swap = swap or {}
                table.insert( swap,cmd)                
            end
        end
    end

    if swap then
        self.cmds = swap
    end

    return true
end

function Battle:CheckCMD(cmd)
    if cmd.type == CmdType.UseSkill then
        return self:_CheckUseSkill(cmd)
    elseif cmd.type == CmdType.HeroUseSkill then
        return self:_CheckUseHeroSkill(cmd)
    elseif cmd.type == CmdType.Skip then
        return self:_CheckSkip(cmd)
    elseif cmd.type == CmdType.SetAuto then
        return 0
    end

    return 0
end

function Battle:_CheckUseSkill(cmd)
    if self.shouldStartRound then
        warn("already round.cmd push failed")
        return -101
    end
    if self.state ~= BattleState.Round0 then
        --状态不对，不是合理的压入时机
        warn("state error")
        return -102
    end

    if self.curRoundRole.userObjId ~= cmd.userObjId then
        warn('userObjId not equal')
        return -103
    end

    local role = self:GetRoleById(cmd.useSkillCmd.roleID)

    if role == nil then
        warn('push cmd failed,role not found',cmd.useSkillCmd.roleID)        
        return -104
    end

    if self.curRoundRole == nil then
        warn("curRoundRole nil")
        return -105
    end

    if self.curRoundRole ~= role then
        warn('push cmd failed,role not match',role.id,self.curRoundRole.id)                
        return -107
    end
    return 0
end

function Battle:_CheckSkip(cmd)
    if self.shouldStartRound then
        warn("already round.cmd push failed")
        return -301
    end
    if self.state ~= BattleState.Round0 then
        --状态不对，不是合理的压入时机
        warn("state error")
        return -302
    end

    if self.curRoundRole.userObjId ~= cmd.userObjId then
        warn('userObjId not equal')
        return -303
    end

    return 0
end

function Battle:_CheckUseHeroSkill(cmd)
    local hero = self:GetRoleById(cmd.useSkillCmd.roleID)
    if hero == nil then
        warn("hero not found",cmd.useSkillCmd.roleID)        
        return -201
    end
    if hero.userObjId ~= cmd.userObjId then
        warn("userObjId not valid",cmd.userObjId,hero.userObjId)        
        return -202
    end

    --新规则，必须是自己的回合
    if self.curRoundRole.side ~= hero.side then
        warn("not self side round.")
        return -203
    end

    --新规则，一回合只能插入一次
    if self.actedHero ~= nil then
        warn("hero can act only once per round.")
        return -204
    end

    if self.state ~= BattleState.Round0 then
        --已经在队列了，不能再次插入
        if self.actQ:IsInQueue(hero) then
            --warn("already cast hero skill")
            --如果角色已经设置了技能，则执行失败
            if hero.skillCmd ~= nil then
                warn("alreay has skill cmd")
                return -203
            end
        else
            --不在队列里，必须先有插入
            warn("not in queue,heroSkill cmd faield")
            return -204
        end
    else
        --当前角色是否是自己的角色
        --UseHeroSkill会立即执行，这里不用去判断了
    end

    return 0

end

function Battle:_ProcessCMD(cmd)
    
    if cmdFuncTb == nil then
        cmdFuncTb = {
            [CmdType.UseSkill] = {Battle.ProcessUseSkillCmd,'useSkillCmd'},
            [CmdType.Skip] = {Battle.ProcessSkipCmd,'skipCmd'},
            [CmdType.HeroAct] = {Battle.ProcessHeroActCmd,'heroActCmd'},
            [CmdType.HeroUseSkill] = {Battle.ProcessHeroSkillCmd,'useSkillCmd'}, 
            [CmdType.SetAITarget] = {Battle.ProcessSetAITarget,'setAITarget'},      
            [CmdType.SetAuto] = {Battle.ProcessSetAuto,'setAuto'},          
        }
    end

    local type  = cmd.type
    local tb = cmdFuncTb[type]
    if tb == nil then
        warn('cmd not support yet:',tostring(type))
        return false
    else
        local ret = tb[1](self,cmd.userObjId,cmd[tb[2]])
        if ret == cmdExeRet.failed then
            warn("prcess cmd failed:",type,tb[2])
        elseif ret == cmdExeRet.notnow then
            --print('prcess cmd not now:',type)
            if self.isSimulating then
                warn("precess cmd failed:",type,tb[2])
                ret = cmdExeRet.failed
            end
        else
            --print('prcess cmd succ:',type,self.state,self.tick)
            --处理cmd成功，记录一下
            if gClientBattle and not self.isSimulating then
                self.cmdRecords = self.cmdRecords or {}
                table.insert(self.cmdRecords,{
                    tick = self.tick,
                    state = self.state,
                    cmd = cmd,
                })
            end
        end

        return true,ret
    end
end

function Battle:ProcessUseSkillCmd(userObjId,cmd)
    --当前state比起是Round
    ---必须是当前行动的角色
    if self.state ~= BattleState.Round then
        return cmdExeRet.notnow
    end

    local role = self:GetRoleById(cmd.roleID)

    if role == nil then
        warn('process cmd failed,role not found',cmd.roleID)        
        return cmdExeRet.failed
    end

    if self.curRoundRole == nil then
        warn("curRoundRole nil")
        return cmdExeRet.failed
    end
    
    if role.userObjId ~= userObjId then
        warn('process cmd failed,userObjId failed',userObjId,role.userObjId)
        return cmdExeRet.failed
    end

    if self.curRoundRole ~= role then
        warn('process cmd failed,role not match',role.id,self.curRoundRole.id)                
        return cmdExeRet.failed
    end

    local ret = role:ActUseSkillByIndex(cmd.skillSelected + 1,cmd.targetSelected)
    
    if ret then
        return cmdExeRet.succ
    else
        return cmdExeRet.failed
    end
end

function Battle:ProcessSkipCmd(userObjId,cmd)
    if self.state ~= BattleState.Round then
        return cmdExeRet.notnow
    end

    local role = self:GetRoleById(cmd.roleID)

    if role == nil then
        warn("role nil",cmd.roleID)
        return cmdExeRet.failed
    end
    
    if role.userObjId ~= userObjId then
        warn('process cmd failed,role not match',userObjId,role.userObjId)                
        return cmdExeRet.failed
    end
    
    if cmd.autoUseSkill then
        role:ProcessAI()
    end

    return cmdExeRet.succ
end

function Battle:ProcessHeroActCmd(userObjId,cmd)
    -- if self.state ~= BattleState.RoundStart then
    --     return cmdExeRet.notnow
    -- end

    error("ProcessHeroActCmd not used!")

    local hero = self:GetHero(cmd.side)
    if hero == nil then
        warn("hero not found",cmd.side)        
        return cmdExeRet.failed
    end

    if not self:IsValidHeroCMD(userObjId,cmd) then
        warn("userObjId not valid",userObjId)
        return cmdExeRet.failed
    end

    --如果是自己符灵的回合，不需要插入
    if self.state == BattleState.Round0 then
        local role = self.curRoundRole
        if role.userObjId == userObjId then
            return cmdExeRet.succ
        end
    else
        --其他状态下，直接插入队列
        --插入到行动队列头
        self.actQ:InsertPriority(hero)
    end

    return cmdExeRet.succ
end

--主角技能cmd，新主角逻辑
function Battle:ProcessHeroSkillCmd(userObjId,cmd)
    --如果是Round0阶段
        --如果是自己的符灵，则执行执行
        --如果是别人的符灵，则记录指令，下回合行动
    --如果是其他阶段
        --记录指令，下回合行动

    --新版本修改，主角技能只能在自己角色的回合使用

    local hero = self:GetRoleById(cmd.roleID)
    if hero == nil then
        warn("hero not found",cmd.roleID)        
        return cmdExeRet.failed
    end
    if hero.userObjId ~= userObjId then
        warn("userObjId not valid",userObjId,hero.userObjId)        
        return cmdExeRet.failed
    end
    --已经在队列了，不能再次插入
    -- if self.actQ:IsInQueue(hero) then
    --     --warn("already cast hero skill")
    --     --如果角色已经设置了技能，则执行失败
    --     if hero.skillCmd ~= nil then
    --         return cmdExeRet.failed
    --     else
    --         --设置技能cmd
    --         hero.skillCmd = {cmd.skillSelected + 1,cmd.targetSelected}
    --         return cmdExeRet.succ
    --     end
    -- end        
    
    local processDone = false

    if self.state == BattleState.Round0 then
        local role = self.curRoundRole
        
        --自己的回合客户端可以判断，发送不同的cmd
        --如果是自己的回合，等价于常规的使用技能指令
        --客户端、服务器的controller控制的状态切换，lua没有能力去切换状态
        --因此如果已经是主角回合了，收到了这个cmd，需要特殊处理，转换成常规使用技能的cmd

        --新版本修改后，主角不会处于自己的回合

        -- if role == hero then            
        --     --转换cmd，等到Round执行
        --     if cmd.skillSelected == -1 then
        --         --特殊，切自动
        --         self:_PushCMD({
        --             type = CmdType.Skip,
        --             userObjId = userObjId,
        --             skipCmd = {
        --                 roleID = role.id,
        --                 autoUseSkill = true,
        --             }
        --         })
        --     else
        --         self:_PushCMD({
        --             type = CmdType.UseSkill,
        --             userObjId = userObjId,
        --             useSkillCmd = cmd,
        --         })
        --     end

        --     return cmdExeRet.succ
        -- end

        if role == hero then
            warn("hero round,should not process ProcessHeroSkillCmd")
            return cmdExeRet.failed
        end

        if role.userObjId == userObjId then
            if cmd.skillSelected == -1 then
                hero:ProcessAI()
                self:ExcuteActions()
            else
                --自己的回合，直接cast技能
                local ret = hero:CastSkillByIndex(cmd.skillSelected + 1,cmd.targetSelected)
                self.actedHero = hero
                if not ret then
                    warn("cast hero skill failed",cmd.skillSelected + 1,cmd.targetSelected)                
                    return cmdExeRet.failed
                end
            end

            --主角技能特殊，处理完后，可能发生状态变化，导致当前回合的人没有目标甚至死亡
            if not role:CheckCanRoundBegin(true) then
                --发生了特殊情况，当前回合的角色已经不能行动了，开始这个回合
                --self.shouldStartRound = true
                self:SetRoundStart()
            end
            --这一波已经清掉了，开始战斗
            --战斗已经有胜负了
            if (self:IsWaveClear() or self:CalcWinSide() ~= BattleSide.bs_Invalid) then
                self:SetRoundStart()
            end
            processDone = true
        else
            --下回合
            processDone = false
        end
    end

    if processDone then
        return cmdExeRet.succ
    else
        --新版本修改，不能在其他状态下使用主角技能了
        return cmdExeRet.faield
    end

    error("hero not enqueue anymore.")

    --记录技能、目标
    --插入队列
    hero.skillCmd = {cmd.skillSelected + 1,cmd.targetSelected}
    self.actQ:InsertPriority(hero)
    return cmdExeRet.succ
end


function Battle:ProcessSetAITarget(userObjId,cmd)

    local target = self:GetRoleById(cmd.targetId)
    if target == nil then
        return cmdExeRet.succ
    end

    if not target.isValid or not target:IsAlive() then
        return cmdExeRet.succ
    end
	
    local roles = self:FindRolesIncludeHero(findAliveByUserObjId,userObjId)
    if roles == nil or #roles == 0 then
        return cmdExeRet.succ
    end
	local role = roles[1]
	if role == nil then
		return cmdExeRet.succ
	end
	--全队共享集火目标
	roles = self:FindRolesIncludeHero(findAliveBySide,role.side)
	
    for _,r in ipairs(roles) do
        r:AI_SetPriorityTarget(target)
    end

    return cmdExeRet.succ
end

function Battle:ProcessSetAuto(userObjId,cmd)

    if self.state ~= BattleState.RoundBegin 
        and self.state ~= BattleState.Round0
        and self.state ~= BattleState.Round then
        return cmdExeRet.notnow
    end

    local roles = self:FindRolesIncludeHero(findAliveByUserObjId,userObjId)
    if roles == nil then
        return cmdExeRet.succ
    end
    for _,r in ipairs(roles) do
        --r.isAuto = cmd.auto
        if r.isHeroCard then
            r.isAuto = cmd.auto
        end
    end
    return cmdExeRet.succ
end

--------------------------------------------

--Action----------------------
function Battle:AddAction(action)
    table.insert(self.actions,action)
end

function Battle:ExcuteActions()
    local q = self.actions
    local hasAction = #q > 0

    while #q > 0 do
        local action = table.remove(q,1)
        self:NewEventGroup()
        if action.actionType == BattleActionType.UseSkill then
            --print(action.roleId .. " do action ")
            local role = self:GetRoleById(action.roleId)
            --role:SendMessage("msgAction",action)
            if role ~= nil then
                role:CastSkill(action.skillId,action.targetId,action.data)
                --技能结束，移除死亡角色
                self:CheckAlive()
                role:SendMessage("msgActionDone",action)
            end
            self:BroadCastMsg("bcActionDone",role,action)
        elseif action.actionType == BattleActionType.UseSkillByIndex then
            local role = self:GetRoleById(action.roleId)
            --role:SendMessage("msgAction",action)
            if role ~= nil then
                role:CastSkillByIndex(action.index,action.targetId,action.data)
                --技能结束，移除死亡角色
                self:CheckAlive()
                role:SendMessage("msgActionDone",action)
            end
            self:BroadCastMsg("bcActionDone",role,action)
        else
            warn("no valid action:",action.actionType)
        end

        local events = self:PopEventGroup()
        local eNum = #events
        --一个action内发生的事件，则构造一个平行事件
        if eNum > 1 then

            local timedEvents = {}

            local addParallel = function()
                if #timedEvents > 0 then
                    self:AddEvent({
                        type = BattleEventType.ParallelEvent,
                        parallelEvent = {
                            events = timedEvents,
                        }
                    })
                    timedEvents = {}
                end
            end
            
            for _,e in ipairs(events) do
                local isTimed  = false
                
                if e.type == BattleEventType.UseSkill then
                    local t,p = self:CalcEventTime(e)
                    if t > 0 then
                        isTimed = true
                    end
                elseif e.type == BattleEventType.ImpactsChange or e.type == BattleEventType.ChangeSkills then
					--changeskill：
					--42号impact产生事件changeskill 在msgAfterUseSkill后，所以没被打包进useskill的hit里，产生了单独事件
					--导致一组平行事件被打断
					--比如山鬼带黑熊怪、祸斗 使用协战，三者中有一个队友可能出手慢
					--ImpactsChange：
					--BuffContainer的使用技能后移除buff也可能打断平行事件
					--比如山鬼带两个监兵（183 带两个1799 ）
					isTimed = true
				end
                --local strlog = string.format( "action.actionType %s e %s isTimed %s",action.actionType,e.type,isTimed )    
                --print(strlog)
                if isTimed then
                    table.insert(timedEvents,e)
                else
                    addParallel()
                    self:AddEvent(e)
                end
            end
            addParallel()
        elseif eNum > 0 then
            self:AddEvent(events[1])
        end
    end

    return hasAction
end

function Battle:_PushCMD(cmd)
    if cmd == nil then
        return
    end
    
    --主角cmd特殊，立即执行
    if cmd.type == CmdType.HeroUseSkill
         or cmd.type == CmdType.HeroAct 
         or cmd.type == CmdType.SetAuto then
        local ret,exeRet = self:_ProcessCMD(cmd)
        if ret then
            if exeRet == cmdExeRet.notnow then
                table.insert( self.cmds,cmd)                
            end
        end
        return
    end

    table.insert(self.cmds,cmd)

    --当前回合是等待输入的回合
    if self.state == BattleState.Round0 then
        --当前指令是使用技能的指令
        if cmd.type == CmdType.UseSkill then
            --当前待行动的角色，是指令里的角色
            if cmd.useSkillCmd.roleID == self.curRoundRole.id then
                --self.shouldStartRound = true
                self:SetRoundStart()
            end
        elseif cmd.type == CmdType.Skip then
            --当前待行动的角色，是指令里的角色
            if cmd.userObjId == self.curRoundRole.userObjId
                and cmd.skipCmd.roleID == self.curRoundRole.id
                and cmd.skipCmd.autoUseSkill then
                --self.shouldStartRound = true
                self:SetRoundStart()
            end
        end
    end
end

--------------------------------------------
--注册pb
local pb = require ('protobuf')

function Battle:PushCMD(buff,len)
    local cmd = pb.decode('ProtobufPacket.BattleCmd',buff,len)

    --检查这个cmd是否合法
    local ret = self:CheckCMD(cmd)
    if ret ~= 0 then
        return ret
    end

    self:_PushCMD(cmd)
    return 0
end

--RoundBegin是否已经OK，可以开始Round
function Battle:ShouldStartRound()
    if self.shouldStartRound then
        return 1
    else
        return 0
    end
end


function Battle:SetRoundStart()
    self.shouldStartRound = true
end

--主角类操作，需要立即给反馈
--返回
--1，当前优先级
local validHeroActState = {
    BattleState.RoundBegin,
    BattleState.Round,
    BattleState.RoundEnd,
    BattleState.RoundStart,
    BattleState.Round0,
}
function Battle:PushHeroAct(side,userObjId)
    
    --主角技能已经不走插入的新式
    error("hero act not used!")

    if type(side) ~= 'number' and type(userObjId) ~= 'number' then
        return -1
    end

    local validState = false
    for _,st in ipairs(validHeroActState) do
        if st == self.state then
            validState = true
            break
        end
    end

    if not validState then
        return -3
    end

    local hero = self:GetHero(side)
    if hero == nil then
        return -1
    end

    if hero == self.curRoundRole then
        --当前行动的是主角，不再接受指令
        if self.state ~= BattleState.Round or self.state ~= BattleState.RoundEnd then
            return -1
        end
    end

    --主角行动需要立即给客户端一个表现，产生一个事件，表现一下下回合角色立即行动
    --单这个时候，实际并没有执行cmd，cmd还是在下次执行roundBegin的时候，按照顺序处理，这样保证逻辑执行的顺序不受时间影响
    --因为逻辑里，时间刻度是回合
    --判断一下是否合法，不合法立即给反馈
    
    --已经在cmd等待队列了，不能再次插入
    local index = 0
    for _,cmd in ipairs(self.cmds) do
        --这个user，已经【成功】插入了一个主角行动cmd
        if cmd.userObjId == userObjId and cmd.type == CmdType.HeroAct then
            return -1 --不嫩再次插入
        end
        if cmd.type == CmdType.HeroAct then
            index = index + 1
        end
    end
    
    local cmd = {side = side}

    if not self:IsValidHeroCMD(userObjId,cmd) then
        return -2
    end

    --检查通过
    self:_PushCMD({
        type = CmdType.HeroAct,
        userObjId = userObjId,
        heroActCmd = cmd,
    })
    
    return index
end

function Battle:IsValidHeroCMD(userObjId,cmd)
    local hero = self:GetHero(cmd.side)
    if hero == nil then
        return false
    end
    if hero.userObjId ~= userObjId then
        return false
    end

    --已经在队列了，不能再次插入
    if self.actQ:IsInQueue(hero) then
        return false
    end

    return true
end

function Battle:AddRole(buff,len)
    local initData = pb.decode("ProtobufPacket.RoleInitData",buff,len)
    if initData == nil then
        return -1
    end
    --print("add role")
    local role = self:CreateRole(initData)
    if role == nil then
        return -1
    end
    return role.id
end

function Battle:SetBattleInitData(buff,len)
    local initData = pb.decode("ProtobufPacket.BattleInitData",buff,len)
    self:_SetBattleInitData(initData)
end

function Battle:_SetBattleInitData(initData)
    if initData == nil then
        return
    end
    self.waveMax = initData.waveMax
    self.blueWaves = {}
    self.redWaves = {}
    self.waveDropTb = {}
    self.enableBubble = initData.enableBubble
	self.arenaLevel = initData.arenaLevel --斗妖场段位
    if rawget(initData,'queryOpts') ~= nil then
        self.queryOpts = initData.queryOpts
    end

    for _,w in ipairs(initData.blues) do
        local waveIndex = w.waveIndex + 1       
        self.blueWaves[waveIndex] = w.roles
    end

    for _,w in ipairs(initData.reds) do
        local waveIndex = w.waveIndex + 1       
        self.redWaves[waveIndex] = w.roles
    end

    for _,w in ipairs(initData.drops) do
        local waveIndex = w.waveIndex + 1       
        self.waveDropTb[waveIndex] = w.drops
    end

    if initData.blueImpacts ~= nil then
        self.blueImpacts = initData.blueImpacts
    end

    if initData.redImpacts ~= nil then
        self.redImpacts = initData.redImpacts
    end

end

function Battle:PullMsg()
    local events = self:PopEventGroup()
    local timeNeed,hasPause = self:CalcEventListTime(events)
    local hasPuaseInt = hasPause

    self:NewEventGroup()
    local msg = {
        events = events,
        hasPause = hasPause,
    }

    local buff = pb.encode('ProtobufPacket.BattleMsg',msg)
    return buff,timeNeed,hasPuaseInt
end

function Battle:LoadWave(waveIndex,side)
    local datas = nil

    local waveDatas = self.blueWaves

    if side == BattleSide.bs_Red then
        waveDatas = self.redWaves
    end

    if waveDatas ~= nil then
        local waveData = waveDatas[waveIndex]
        if waveData ~= nil then
            -- if waveData.waveType == 1 then
            --     datas = waveData.roles
            -- else
            --     local waveId = waveData.battleWaveId
            --     datas = self:LoadRoleFromTab(waveId,waveIndex,side)
            -- end
            datas = waveData
        end
    end

    local createRole = function(data)
        local role = self:CreateRole(data,true)
        if role ~= nil then
            --根据波次分配掉落
            role.drop = self:GetDropFromWave(waveIndex,role.battlePos,role.battlePosArea)
        end
    end

    if datas ~= nil then
        for _,data in ipairs(datas) do
            createRole(data)
        end
    end
    
    --把表里配置的角色刷出来,如果位置上有人，则不刷出来
    --这个角色可能是怪，也可能是助阵角色
    local tabDatas = self:LoadWaveFromTab(waveIndex,side)
    if tabDatas ~= nil then
        for _,data in ipairs(tabDatas) do
            --临时方案，最终方案没确定
            if side == BattleSide.bs_Blue then
                data.userObjId = 1
            end
            createRole(data)
        end
    end

end


function Battle:GetBattleStatus()
    local msg = {
        isWaveClear = self:IsWaveClear(),
        winSide = self:CalcWinSide(),
        waveIndex = self.waveIndex,
        waveMax = self.waveMax,
    }
    return pb.encode('ProtobufPacket.BattleStatus',msg)
end

function Battle:GetBattleResult()

    local msg = {
        winSide = self:CalcWinSide(),
        collectedPreviewDropIds = self.collectedPreviewDropIds,
        seed = self.rand.seed,
        roundCount = self.totalRoundCount,
        datas = self:SavedDataToArray(),

        blueStatist = {
            totalDamage = self.blueStatist.totalDamage,
            totalHeal = self.blueStatist.totalHeal,
            maxDamage = self.blueStatist.maxDamage,
            maxDamageUnclamp = self.blueStatist.maxDamageUnclamp,
            deadCount = self.blueStatist.deadCount,
			totalDamageLastWave = self.blueStatist.totalDamageLastWave,
        },
    }

    if self.queryOpts and self.queryOpts.redStatist then
        msg.redStatist = {
            totalDamage = self.redStatist.totalDamage,
            totalHeal = self.redStatist.totalHeal,
            maxDamage = self.redStatist.maxDamage,
            maxDamageUnclamp = self.redStatist.maxDamageUnclamp,
            deadCount = self.redStatist.deadCount,
			totalDamageLastWave = self.redStatist.totalDamageLastWave,
        }

    end

    if self.statistRoles == nil then
        return pb.encode('ProtobufPacket.BattleResult',msg)
    end

    --没有额外的查询了
    if self.queryOpts == nil then
        return pb.encode('ProtobufPacket.BattleResult',msg)
    end

    local hasBlueRoleOpts = rawget(self.queryOpts,"blueRoleOpts")
    local hasRedRoleOpts = rawget(self.queryOpts,"redRoleOpts")

    if self.queryOpts.roleBasics 
            or hasBlueRoleOpts or hasRedRoleOpts
            or self.queryOpts.blueKill or self.queryOpts.redKill
            then 
        
        --人员基本信息
        local roleBasics = {}
        for _,r in pairs(self.statistRoles) do
            local t = {
                queryId = r.queryId,

                side = r.side,
                cardId = r.cardId,
                monsterId = r.monsterId,
                roleBaseId = r.roleBaseID,
                maxHp = r.maxHp,
            }

            local hp = 0
            local role = self:GetRoleById(r.queryId)
            if role ~= nil and role.isValid and role:IsAlive() then
                hp = role:GetHP()
            end

            t.hp = hp

            table.insert(roleBasics,t)
        end
        msg.roles = roleBasics
    end

    local tb2arry = function(tb)
        local list = {}
        for k,v in pairs(tb) do
            table.insert(list,{id = k,value = v})
        end
        return list
    end

    if hasBlueRoleOpts or hasRedRoleOpts then
        
        --人员详细信息
        local blueRoleDetails = {}
        local redRoleDetails = {}

        for _,r in pairs(self.statistRoles) do

            local opt = self.queryOpts.blueRoleOpts
            if r.side == BattleSide.bs_Red then
                opt = self.queryOpts.redRoleOpts
            end

            if opt ~= nil then
                            
                local t = {
                    queryId = r.queryId,

                    actCount = r.actCount,
                    totalDamage = r.totalDamage,
                    totalHeal = r.totalHeal,
                    maxDamage = r.maxDamage,
                    deadRound = r.deadRound,
					totalDamageLastWave = r.totalDamageLastWave,
                }

                if opt.skillDamage then
                    t.skillDamages = tb2arry(r.skillDamageTb)
                end
                if opt.skillHeal then
                    t.skillHeals = tb2arry(r.skillHealTb)
                end

                if opt.impactDamge then
                    t.impactDamages = tb2arry(r.impactDamageTb)
                end
                if opt.impactHeal then
                    t.impactHeals = tb2arry(r.impactHealTb)
                end

                if opt.hitTypeDamage then
                    t.hitTypeDamages = tb2arry(r.hitTypeDamageTb)
                end
                if opt.hitTypeHeal then
                    t.hitTypeHeals = tb2arry(r.hitTypeHealTb)
                end

                if r.side == BattleSide.bs_Blue then
                    table.insert(blueRoleDetails,t)
                else
                    table.insert(redRoleDetails,t)
                end

            end
        end

        if self.queryOpts ~= nil and hasBlueRoleOpts then
            msg.blueRoleStatics = blueRoleDetails
        end
        if self.queryOpts ~= nil and hasRedRoleOpts then 
            msg.redRoleStatics = redRoleDetails
        end

    end


    --击杀信息
    local makeKillList = function(l)
        local t = {}
        for _,v in ipairs(l) do
            table.insert(t,{id=v[1],value=v[2]})
        end
        return t
    end
    
    if self.blueStatist.killed ~= nil and self.queryOpts.blueKill then
        msg.blueKillStatics = makeKillList(self.blueStatist.killed)
    end

    if self.redStatist.killed ~= nil and self.queryOpts.redKill then
        msg.redKillStatics = makeKillList(self.redStatist.killed)
    end

    return pb.encode('ProtobufPacket.BattleResult',msg)
end

function Battle:GetCmdRecords()
    local msg = {
        records = self.cmdRecords
    }
    return pb.encode("ProtobufPacket.CmdRecordList",msg)
end

function Battle:CalcWinSide()

    --先结算强制
    if self.forceWinSide ~= nil then
        return self.forceWinSide
    end

    --再结算全灭的情况
    local blueLiveCount = self:CountRoles(findAliveBySide,BattleSide.bs_Blue)
    if blueLiveCount <= 0 then
        return BattleSide.bs_Red
    end

    if self:IsWaveClear() and self.waveIndex >= self.waveMax then
        return BattleSide.bs_Blue
    end

    --最后结算特殊情况(boss击杀等)
    if self.priWinSide ~= nil then
        return self.priWinSide
    end

    return BattleSide.bs_Invalid
end

function Battle:ForceFinish(winSide)
    self.forceWinSide = winSide or BattleSide.bs_Blue
end


function Battle:OnRoleDead(role,killerId)

    if role == nil then
        return
    end

    if not role.dead then
        return
    end

    self.deadRoleTb = self.deadRoleTb or {}
    self.deadRoleTb[role.id] = role

    self:StatistKill(killerId,role)

end

function Battle:OnAvatarPartsMainDead(role,killerId)
	if role == nil then
		return
	end
	if not role.dead then
		return
	end

	 --通知Boss其它分部
    for _,r in ipairs(self.roles) do
        if r.isValid then
           r:OnAvatarPartsMainDead(role,killerId)
        end
    end
end

function Battle:HasRoleDead(side)
    if self.deadRoleTb == nil then
        return false
    end

    for _,role in pairs(self.deadRoleTb) do
        if role.side == side then
            return true
        end
    end
    return false
end

function Battle:CountRoleDead(side)
    if self.deadRoleTb == nil then
        return 0
    end

    local count = 0
    for _,role in pairs(self.deadRoleTb) do
        if role.side == side then
            count = count + 1
        end
    end
    return count
end

function Battle:IsDeadRole(id)
    if self.deadRoleTb == nil then
        return false
    end

    local deadRole = self.deadRoleTb[id]
    if deadRole == nil then
        return false
    end
    return true
end


--复活某个已死亡角色
function Battle:Relive(id,hpPercent)
    if self.deadRoleTb == nil then
        return
    end

    local deadRole = self.deadRoleTb[id]
    if deadRole == nil then
        return
    end

    --是否可以复活
    if not deadRole:CanRelive() then
        return
    end

    local initData = deadRole.cachedInitData
    if initData == nil then
        return
    end

    --位置上是否有人
    local orgRole = self:GetRole(getByBattlePos,initData.battlePosArea,initData.battlePos)
    if orgRole ~= nil then
        return
    end

    -- local id = self.idBase + 1
    -- self.idBase = id
    local role = self:_CreateRole(initData,id)

    --使用被动，上buff，加入行动队列
    self:_OnRoleCreate(role)
    
    --更新血量
    if hpPercent <= 0 then
        role:SetHP(role:GetMaxHP())
    else
        local hpGain = math.ceil( role:GetMaxHP() * (hpPercent / 10000) )
        role:SetHP(hpGain)
    end

    self:NotifyRoleCreate(role)

    return role
end

local FinType = { 
    ClearAll = 0,
    KillBoss = 1,
    KillProtege = 2,  --击杀被保护者
}

function Battle:OnSpecRoleKilled(role,roleType)

    if roleType == RoleType.Boss
        or roleType == RoleType.Protege then

        local tab = tabMgr.GetByID('Battle',self.battleId)
        if tab == nil then
            return
        end

        if tab.FinType == FinType.KillBoss then
            --击杀boss，boss的对立面获胜
            if role.side == BattleSide.bs_Red then
                self.priWinSide = BattleSide.bs_Blue
            else
                self.priWinSide = BattleSide.bs_Red
            end
        elseif tab.FinType == FinType.KillProtege then
            --击杀被保护者，被保护者对立面获胜
            if role.side == BattleSide.bs_Red then
                self.priWinSide = BattleSide.bs_Blue
            else
                self.priWinSide = BattleSide.bs_Red
            end
        end
    end

end

function Battle:Sync()
    self:AddEvent( {
        type = BattleEventType.SyncState,
        syncEvent = self:GetSync(),
    })
end

function Battle:GetSync(isSyncAllAttrs)
    local syncInfo = {
        battleSync = {
            spirit = self.spirit,
            spiritMax = self.spiritMax,
            envID = self:GetCurEnvId(),
            waveIndex = self.waveIndex,
            roundCount = self.roundCount,
        },
        roleSyncs = {
        },
		aiTargetSync = {
		},
    }
	local userObjIds = {}
    for _,role in ipairs(self.roles) do
        if role.isValid then
            table.insert( syncInfo.roleSyncs, role:GetSync(isSyncAllAttrs))
			
			--同步集火目标
			if role.userObjId ~= nil and userObjIds[role.userObjId] == nil then
				--同userObjId只处理一次
				userObjIds[role.userObjId] = true
				
				local aiTarget = {}
				aiTarget.userObjId = role.userObjId
				if role.aiOurPriorityTarget ~= nil and role.aiOurPriorityTarget.isValid then
					aiTarget.aiOurPriorityTarget = role.aiOurPriorityTarget.id
				end
				if role.aiPriorityTarget ~= nil and role.aiPriorityTarget.isValid then
					aiTarget.aiPriorityTarget = role.aiPriorityTarget.id
				end
				--有目标才同步
				if aiTarget.aiOurPriorityTarget ~= nil or  aiTarget.aiPriorityTarget ~=nil then
					table.insert( syncInfo.aiTargetSync, aiTarget)
				end
			end
        end
    end
    return syncInfo
end

--环境相关
function Battle:InitEnv()
    self.spirit = 0
    self.curEnvType = -1
    
    local tab = tabMgr.GetByID('Battle',self.battleId)
    self.spirit = tab.DefaultSpirit
    self.spiritMax = tab.SpiritMax

    if tab ~= nil then
        local defEnv = tab.DefaultEnvType
        if defEnv ~= -1 then
            self.hasDefEnv = true
            self.curEnvType = defEnv
        end
    end
end

function Battle:CalcEnv()

    --按照规则初始化
    --获取双方人妖数量差
    local envCounter0 = self:CountEnvRole(0)
    local envCounter1 = self:CountEnvRole(1)
    
    if envCounter0 > envCounter1 then
        self.curEnvType = 0
    elseif envCounter0 < envCounter1 then
        self.curEnvType = 1
    else
        if self:IsRandLt(5000) then
            self.curEnvType = 0
        else
            self.curEnvType = 1
        end
    end

    return envCounter0,envCounter1
end

function Battle:GetCurEnvId()
    local tab = tabMgr.GetByID('Battle',self.battleId)
    if self.curEnvType == -1 then
        return -1
    elseif self.curEnvType == 0 then
        return tab.DayEnvId
    else
        return tab.NightEvnId
    end
end

function Battle:IncSpirit(val)
    self:SetSpirit(self.spirit + val)
end

function Battle:SetSpirit(val)
    self.spirit = common.clamp(val,0,self.spiritMax)
    self:OnSpiritChange()
end

function Battle:SetSpiritMax(val)
    self.spiritMax = common.clamp(val,0,val)
    self:OnSpiritChange()
end

function Battle:OnSpiritChange()
    --消耗掉之前也同步一下
    self:AddEvent({
        type = BattleEventType.ChangeSpirit,
        changeSpiritEvent = {
            cur = self.spirit,
            max = self.spiritMax,
        }
    })

    local changeEnv = false
    --集满之后清0
    if self.spirit >= self.spiritMax then
        self.spirit = self.spirit - self.spiritMax
        changeEnv = true
    end
    self.spirit = common.clamp(self.spirit,0,self.spiritMax)

    if changeEnv then
        self:ToggleEnv()

        self:AddEvent({
            type = BattleEventType.ChangeSpirit,
            changeSpiritEvent = {
                cur = self.spirit,
                max = self.spiritMax,
            }
        })

    end
end

function Battle:ChangeEnvType(type,isSilent)

    if type ~= 0 and type ~= 1 then
        return
    end

    self.curEnvType = type

    --策划需求，如果战斗结束了，那么不触发这次换届切换
    if self:IsWaveClear() and self:CalcWinSide() ~= BattleSide.bs_Invalid then
        return
    end

    self:BroadCastMsg("bcChangeEnv",self.curEnvType)


    if not isSilent then
        self:AddEvent({
            type = BattleEventType.ChangeEnv,
            changeEnvEvent = {
                envID = self:GetCurEnvId(),
            }
        })
    end
end

function Battle:ToggleEnv()
    if self.curEnvType == 0 then
        self:ChangeEnvType(1)
    elseif self.curEnvType == 1 then
        self:ChangeEnvType(0)
    end
end

function Battle:IsWaveClear()
    local blueLiveCount = self:CountRoles(findAliveBySide,BattleSide.bs_Blue)
    if blueLiveCount <= 0 then
        return true
    end
    local redLiveCount = self:CountRoles(findAliveBySide,BattleSide.bs_Red)
    if redLiveCount <= 0 then
        return true
    end
    return false
end

function Battle:BroadCastMsg(msg,...)
    --通知场上的所有人
    for _,role in ipairs(self.roles) do
        if role.isValid then
            role:SendMessage(msg,...)
        end
    end
end

function Battle:GetDropFromWave(waveIndex,battlePos,battlePosArea)
    if self.waveDropTb == nil then
        return nil
    end

    local waveDrop = self.waveDropTb[waveIndex]
    if waveDrop == nil then
        return nil
    end

    local dropGot = nil
    local index = -1
    for i,drop in ipairs(waveDrop) do
        if drop.battlePosArea == battlePosArea and drop.battlePos == battlePos then
            dropGot = drop
            index = i
        end
    end
    if dropGot ~= nil then
        table.remove( waveDrop, index)
    end
    return dropGot
end

function Battle:CollectDrop(drop)
    if drop ~= nil then
        table.insert(self.collectedPreviewDropIds,drop.previewDropId)
    end
end

--广播
function Battle:SendMessage(func,...)
    if self.listeners == nil then
        return
    end

    for _,listener in ipairs(self.listeners) do
        if listener[func] ~= nil then
            listener[func](listener,self,...)
        end
    end
end

function Battle:RegisterListener(listener)
    self.listeners = self.listeners or {}
    table.insert(self.listeners,listener)
end

function Battle:LoadRoleBase(roleId)
    local tab = tabMgr.GetByID('RoleBaseAttr',roleId)
    if tab == nil then
        return nil
    end

    local roleInfo = {}
    roleInfo.roleBaseID = roleId

    self:LoadAttrs(tab,roleInfo)

    roleInfo.cardId = tab.CardId

    --roleInfo.canReplace = true

    return roleInfo
end

function Battle:LoadAttrs(tab,roleInfo)
    
    --属性
    if tab.AttrExID ~= -1 then
        local attrTab = tabMgr.GetByID('RoleAttrEx',tab.AttrExID)
        if attrTab == nil then
            return nil
        end
        local attrsMsg = { attrs = {}}
        local attrs = attrsMsg.attrs
        --需要从表里初始化的属性
        local initAttrs = {
            'MaxHP',
            'Attack',
            'Defense',
            'Speed',
            'CritChance',
            'CritEffect',
            'ImpactChance',
            'ImpactResist',
        }
        for _,attrName in ipairs(initAttrs) do
            table.insert( attrs,{type = AttrType[attrName],value = attrTab[attrName]} )
        end
        roleInfo.attrs = attrsMsg
    end

    --技能
    local hasSkill = false
    for _,skillId in ipairs(tab.Skill) do
        if skillId >= 0 then
            hasSkill = true
            break
        end
    end

    if hasSkill then
        roleInfo.skillInfos = {}
        for _,skillId in ipairs(tab.Skill) do
            if skillId >= 0 then
                table.insert( roleInfo.skillInfos,{
                    skillID = skillId,
                    level = 1,
                })
            end
        end
    end

end

function Battle:LoadRoleFromTab(monsterId,side,battlePos,battlePosArea)
    local monsterTab = tabMgr.GetByID("Monster",monsterId)
    if monsterTab == nil then
        return nil
    end
    local roleInfo = self:LoadRoleBase(monsterTab.RoleBaseID)
    if roleInfo == nil then
        return nil
    end

    --读取一下monster扩展修改的属性和技能
    self:LoadAttrs(monsterTab,roleInfo)

    local initData = {}
    initData.roleInfo = roleInfo
    initData.battlePos = battlePos
    initData.side = side
    initData.spawnRule = SpawnRule.FailedIfHold
    initData.ai = monsterTab.AI
    if battlePosArea == nil then
        if side == BattleSide.bs_Red then
            initData.battlePosArea = BattleAreaType.Red
        elseif side == BattleSide.bs_Blue then
            initData.battlePosArea = BattleAreaType.Blue
        else
            initData.battlePosArea = BattleAreaType.Red
        end
    else
        initData.battlePosArea = battlePosArea
    end
    initData.roleInfo.monsterId = monsterId

    return initData
end

function Battle:LoadRoleFromWaveTab(waveId,side)
    local tab = tabMgr.GetByID('BattleWave',waveId)
    if tab == nil then
        return nil
    end

    local datas = {}

    local load = function(tb,heroMonsterId)
        for i,monsterId in ipairs(tb) do
            if monsterId ~= -1 then
                local initData = self:LoadRoleFromTab(monsterId,side,i-1)
                if initData ~= nil then
                    table.insert( datas, initData )
                end
            end
        end

        if heroMonsterId ~= -1 then
            local areaType = BattleAreaType.BlueHero
            if side == BattleSide.bs_Red then
                areaType = BattleAreaType.RedHero
            end
            local initData = self:LoadRoleFromTab(heroMonsterId,side,0,areaType)
            if initData ~= nil then
                initData.roleInfo.isHeroCard = true
                table.insert( datas, initData )
            end
        end
    end

    if side == BattleSide.bs_Blue then
        load(tab.Blue,tab.BlueHero)
    else
        load(tab.Red,tab.RedHero)
    end

    return datas
end


function Battle:LoadWaveFromTab(waveIndex,side)
    local tab = tabMgr.GetByID('Battle',self.battleId)
    if tab == nil then
        return nil
    end
    return self:LoadRoleFromWaveTab(tab.Wave[waveIndex],side)
end

function Battle:GetWaveTab(waveIndex)
    local tab = tabMgr.GetByID('Battle',self.battleId)
    if tab == nil then
        return nil
    end
    local waveTab = tabMgr.GetByID('BattleWave',tab.Wave[waveIndex])
    return waveTab
end

function Battle:PullSyncMsgFull()
    local buff = pb.encode('ProtobufPacket.SyncBattleState',self:GetSync(true))
    return buff
end

function Battle:PullSyncMsg()
    local buff = pb.encode('ProtobufPacket.SyncBattleState',self:GetSync())
    return buff
end

function Battle:Notice(notice)
    self:AddEvent({
        type = BattleEventType.NoticeEvent,
        noticeEvent = {
            notice = notice,
        }
    })
end

function Battle:PlayCutscene(id)

    if self.isMultiPlayer then
        return
    end

    if id == -1 then
        return
    end

    self:AddEvent({
        type = BattleEventType.PlayCutscene,
        playCutsceneEvent = {
            id = id;
        },
    })
end

function Battle:PushGM(gmstr)
    --解析gm
    local vals = common.str_split(gmstr,":")
    if vals[1] == 'k' then
        local side,battlePos = tonumber(vals[2]),tonumber(vals[3])
        local area
        if side == 1 then area = 0 else area = 2 end
        local role = self:GetRole(getByBattlePos,area,battlePos)
        if role ~= nil then
            role:Dead(-1)
            self:Sync()
        end
    elseif vals[1] == "kk" then
        local roleId = tonumber(vals[2])
        local killerId = tonumber(vals[3])
        local role = self:GetRoleById(roleId)
        if role ~= nil then
            role:Dead(killerId)
            self:Sync()
        end
    elseif vals[1] == 'f' then
        local side = tonumber(vals[2])
        self:ForceFinish(side)
    elseif vals[1] == 'd' then
        local side,battlePos = tonumber(vals[2]),tonumber(vals[3])
        local area
        if side == 1 then area = 0 else area = 2 end
        local role = self:GetRole(getByBattlePos,area,battlePos)
        if role ~= nil then
            role:RecvDamage({
                impact = nil,
                value = tonumber(vals[4]),
                senderId = -1,
                hitType = HitType.Damage,
            })
        end
    elseif vals[1] == 's' then
        local side,sp = tonumber(vals[2]),tonumber(vals[3])
    elseif vals[1] == "ff" then
        --全员满血
        local roles = self.roles
        for _,role in ipairs(roles) do
            if role.isValid then
                role:SetHP(role:GetMaxHP())
                role:ClearAllCooldown()
            end
        end
        self:Sync()
    elseif vals[1] == 'auto_relive' then
        --自动回血
        local open = tonumber(vals[2]) == 1
        local roles = self.roles
        for _,role in ipairs(roles) do
            --先上一个清CD，再上一个不产生CD
            if role.isValid then
                if open then
                    role:SetHP(role:GetMaxHP())
                    self.gm_HPAutoGain = true
                else
                    self.gm_HPAutoGain = nil
                end
            end
        end
        self:Sync()
    elseif vals[1] == 'no_cd' then
        --无cd
        local open = tonumber(vals[2]) == 1
        local roles = self.roles
        for _,role in ipairs(roles) do
            --先上一个清CD，再上一个不产生CD
            if role.isValid then
                if open then
                    role:ClearAllCooldown()
                    self.gm_noCD = true
                else
                    self.gm_noCD = nil
                end
            end
        end
        self:Sync()
    elseif vals[1] == 'env' then
        --切人妖
        local env = tonumber(vals[2])
        if env == nil then
            self:ToggleEnv()
        else
            self:ChangeEnvType(env)
        end
    elseif vals[1] == 'inc_sp' then
        local side = tonumber(vals[2])
        local hero = self:GetHero(side)
        if hero ~= nil then
            hero:GainSP(tonumber(vals[3]))
        end
    elseif vals[1] == 'chance_100' then
        if self.gm_Chance10000 then
            self.gm_Chance10000 = nil
        else
            self.gm_Chance10000 = true
        end
    elseif vals[1] == 'impact_chance_100' then
        if self.gm_ImpactChanceFull then
            self.gm_ImpactChanceFull = nil
        else
            self.gm_ImpactChanceFull = true
        end
    elseif vals[1] == 'test_summon' then
        local blues = self:FindRoles(findBySide,BattleSide.bs_Blue)
        local role = blues[1]
        role:TestSummon()
    elseif vals[1] == "debuglog" then
        if _G.debuglogEnable then
            _G.debuglogEnable = nil
            pcall = _G.__pcall
        else
            _G.debuglogEnable = true
            _G.__pcall = _G.__pcall or pcall
            pcall = function(func,...)
                return true,func(...)
            end
        end
    elseif vals[1] == "test_impact" then
        local targetId = tonumber(vals[2])
        local impactId = tonumber(vals[3])
        local senderId = tonumber(vals[4])

        local target = self:GetRoleById(targetId)
        if not target.isValid then
            return
        end

        local sender = nil
        if (senderId ~= -1) then
            sender = self:GetRoleById(senderId)
            if not sender.isValid then
                return
            end
        end

        if impactId < 0 then
            local impact = require("ScriptSkill/ScriptImpact/TestImpact")
            Impact.SendImpactToTarget(impact,target,sender)
        else
            Impact.SendImpactToTarget(impactId,target,sender)
        end
    elseif vals[1] == "test_passive_skill" then
        local targetId = tonumber(vals[2])
        local skillId = tonumber(vals[3])

        local target = self:GetRoleById(targetId)
        if not target.isValid then
            return
        end
        target:CastSkill(skillId,targetId)
    elseif vals[1] == "relive" then
        local targetId = tonumber(vals[2])
        self:Relive(targetId)
    elseif vals[1] == 'test_cast_skill' then
        local casterId = tonumber(vals[2])
        local skillId = tonumber(vals[3])
        local targetId = tonumber(vals[4])

        if targetId == -1 then targetId = casterId end

        local caster = self:GetRoleById(casterId)
        if caster ~= nil then
            caster:CastSkill(skillId,targetId)
        end
    elseif vals[1] == 'test_cast_ss' then
        -- local ScriptSkillParser = require("BattleCore/SkillProcess/ScriptSkillParser")

        -- local casterId = tonumber(vals[2])
        -- local scriptId = tonumber(vals[3])
        -- local targetId = tonumber(vals[4])
        -- local paramId = tonumber(vals[5])
        
        -- local ss = require("ScriptSkill/SS_" .. scriptId)

        -- local sk = ScriptSkillParser.deepParse(ss,paramId,1)

        -- if targetId == -1 then targetId = casterId end

        -- local caster = self:GetRoleById(casterId)
        -- if caster ~= nil then
        --     caster:CastSkill(sk,targetId)
        -- end
    end
end

function Battle:CaclRetHash()
end

function Battle:SetIsMultiPlayer(isMultiPlayer)
    self.isMultiPlayer = isMultiPlayer == 1
end

function Battle:GetValidRolePos(battlePosArea)
    if self.waveValidPos == nil then
        return nil
    end
    return self.waveValidPos[battlePosArea]
end

function Battle:SaveData(key,value)
    self.dataTb = self.dataTb or {}
    self.dataTb[key] = value
end

function Battle:SavedDataToArray()
    if self.dataTb == nil then
        return nil
    end

    local t = {}
    for k,v in pairs(self.dataTb) do
        table.insert(t,{key=k,value=v})
    end
    return t
end

--记录一个角色
function Battle:StatistRole(r)
    self.statistRoles = self.statistRoles or {}

    --已经记录过了
    if self.statistRoles[r.id] ~= nil then
        return
    end

    self.statistRoles[r.id] = {
        queryId = r.id,

        actCount = 0,
        totalDamage = 0,
        totalHeal = 0,
        maxDamage = 0,
        deadRound = -1,
        maxHp = r:GetMaxHP(),

        skillDamageTb = {},
        impactDamageTb = {},
        hitTypeDamageTb = {},

        skillHealTb = {},
        impactHealTb = {},
        hitTypeHealTb = {},

        side = r.side,
        cardId = r.cardId,
        monsterId = r.monsterId,
        roleBaseID = r.roleBaseID,
        totalDamageLastWave = 0,
    }
end

function Battle:StatistDamage(ret)
    if ret == nil then return end
    local sender = self:GetRoleById(ret.senderId)

    if sender == nil then
        return
    end

    if (ret.validValue <= 0) then
        return
    end

    --整体统计

    local statist = self.blueStatist

    if sender.side == BattleSide.bs_Red then
        statist = self.redStatist
    end

    local validValue = ret.validValue
    statist.maxDamage = statist.maxDamage or 0
    if validValue > statist.maxDamage then
        statist.maxDamage = validValue
    end
    
    statist.maxDamageUnclamp = statist.maxDamageUnclamp or 0
    if ret.value > statist.maxDamageUnclamp then
        statist.maxDamageUnclamp = ret.value
    end

    statist.totalDamage = (statist.totalDamage or 0) + validValue
	--如果是最后一波
	if self.waveIndex == self.waveMax then
		statist.totalDamageLastWave = (statist.totalDamageLastWave or 0) + validValue
	end
	
    --角色伤害统计
    local roleTb = self.statistRoles[sender.id]
    if roleTb == nil then
        return
    end
    roleTb.totalDamage = roleTb.totalDamage + validValue
    if validValue > roleTb.maxDamage then
        roleTb.maxDamage = validValue
    end
	--最后一波总伤害
	if self.waveIndex == self.waveMax then
		roleTb.totalDamageLastWave = roleTb.totalDamageLastWave + validValue
	end

    --伤害细节统计
    if ret.impact ~= nil then
        --impact 伤害量
        roleTb.impactDamageTb[ret.impact.impactId] = (roleTb.impactDamageTb[ret.impact.impactId] or 0) + validValue
        --技能伤害量
        if ret.impact.skillInfo ~= nil and ret.impact.skillInfo.skillExId ~= nil then
            local skillId = ret.impact.skillInfo.skillExId
            roleTb.skillDamageTb[skillId] = (roleTb.skillDamageTb[skillId] or 0) + validValue
        end
    end

    --根据hitType统计
    if ret.hitType ~= nil then
        roleTb.hitTypeDamageTb[ret.hitType] = (roleTb.hitTypeDamageTb[ret.hitType] or 0) + validValue
    end

end

function Battle:StatistHeal(ret)
    if ret == nil then return end
    local sender = self:GetRoleById(ret.senderId)

    if sender == nil then
        return
    end

    if ret.validValue <= 0 then
        return
    end

    --整体统计

    local statist = self.blueStatist

    if sender.side == BattleSide.bs_Red then
        statist = self.redStatist
    end

    local validValue = ret.validValue

    statist.totalHeal = (statist.totalHeal or 0) + validValue

    --角色统计
    local roleTb = self.statistRoles[sender.id]
    if roleTb == nil then
        return
    end
    roleTb.totalHeal = roleTb.totalHeal + validValue


    --细节统计
    if ret.impact ~= nil then
        --impact
        roleTb.impactHealTb[ret.impact.impactId] = (roleTb.impactHealTb[ret.impact.impactId] or 0) + validValue
        --技能
        if ret.impact.skillInfo ~= nil and ret.impact.skillInfo.skillExId ~= nil then
            local skillId = ret.impact.skillInfo.skillExId
            roleTb.skillHealTb[skillId] = (roleTb.skillHealTb[skillId] or 0) + validValue
        end
    end

    --根据hitType统计
    if ret.hitType ~= nil then
        roleTb.hitTypeHealTb[ret.hitType] = (roleTb.hitTypeHealTb[ret.hitType] or 0) + validValue
    end
end

function Battle:StatistKill(killerId,role)
    if role == nil then
        return
    end

    self:StatistDead(role)

    --角色统计
    local roleTb = self.statistRoles[role.id]
    if roleTb == nil then
        return
    end
    roleTb.deadRound = self.totalRoundCount

    local killer = self:GetRoleById(killerId)
    if killer == nil then
        return
    end

    local statist = self.blueStatist

    if killer.side == BattleSide.bs_Red then
        statist = self.redStatist
    end

    statist.killed = statist.killed or {}
    table.insert(statist.killed,{role.id,killerId})
end

function Battle:StatistDead(role)
    --死亡数统计
    local statist = self.blueStatist

    if role.side == BattleSide.bs_Red then
        statist = self.redStatist
    end

    statist.deadCount = (statist.deadCount or 0) + 1
end

return Battle