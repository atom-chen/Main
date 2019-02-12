--行动队列

require("class")

local ActQueue = class("ActQueue")

local sortMethod = function(a,b)
    if a == nil then return true end
    if b == nil then return false end

    if a.ap ~= b.ap then
        return a.ap < b.ap
    -- elseif a.hp ~= b.hp and a.ap < 1000 then
    --     return a.hp > b.hp
    else
        return a.battlePos > b.battlePos
    end
end

function ActQueue:ctor()
    self.actQ = {}
    self.priorityQ = {}
end

function ActQueue:Enqueue(role)
    --普通队列，只能存在一个
    if self:IsInQueue(role) then
        return
    end
    table.insert( self.actQ,role)
    self:ReSort()
end

function ActQueue:Dequeue()
    local role = nil
    local pq = self.priorityQ
    if pq[1] ~= nil then
        role = table.remove(pq,1)
    else
        local q = self.actQ;
        role = q[#q]
        table.remove(q,#q)
    end

    return role
end

function ActQueue:Peek()
    --先检查优先队列
    local pq = self.priorityQ
    if pq[1] ~= nil then
        return pq[1]
    end

    local q = self.actQ
    return q[#q]
end

function ActQueue:ReSort()
    local q = self.actQ
    table.sort( q,sortMethod )
end

function ActQueue:Clear()
    self.actQ = {}
    self.priorityQ = {}
end

function ActQueue:IsEmpty()
    return #self.actQ <= 0 and #self.priorityQ <= 0
end

--插入行动，行动按照入队列，先进先出
--单独一个队列，不参与排序，不空时，必然先动
function ActQueue:InsertPriority(role,ignoreHero)
    if role.isHeroCard or ignoreHero then
        table.insert(self.priorityQ,role)
    else
        --插到主角后面
        local pq = self.priorityQ
        local index = 1
        for i,r in ipairs(pq) do
            index = i            
            if not r.isHeroCard then
                break
            end
        end
        table.insert(pq,index,role)
    end
end

--从普通队列里移除
function ActQueue:Remove(role)
    local q = self.actQ
    for i,r in ipairs(q) do
        if role == r then
            table.remove(q,i)
            return true
        end
    end
    return false
end

--获取行动顺序
function ActQueue:GetActIndex(role)
    local pq = self.priorityQ
    local pqCount = #pq


    for i,r in ipairs(pq) do
        if r == role then
            return i - 1
        end
    end

    local q = self.actQ
    local count = #q + pqCount
    for i,r in ipairs(q) do
        if r == role then
            return count - i
        end
    end


    return -1
end

function ActQueue:IsInQueue(role)
    local pq = self.priorityQ
    local q = self.actQ

    for _,r in ipairs(pq) do
        if role == r then
            return true
        end
    end

    for _,r in ipairs(q) do
        if role == r then
            return true
        end
    end

    return false
end

function ActQueue:IsInPriorityQueue(role)
    local pq = self.priorityQ
    for _,r in ipairs(pq) do
        if role == r then
            return true
        end
    end
    return false
end

function ActQueue:Count()
    return #self.actQ,#self.priorityQ
end

function ActQueue:Shuffle()
end

return ActQueue

-- local q = new(ActQueue)
-- q:Init()

-- local r0 = {ap = 100,hp = 1000,id = 1}
-- local r1 = {ap = 110,hp = 1000,id = 2}
-- local r2 = {ap = 110,hp = 900,id = 3}

-- q:Enqueue(r1)
-- q:Enqueue(r0)
-- q:Enqueue(r2)

-- while not q:IsEmpty() do
--     local r = q:Dequeue()
--     print(r.id,r.ap,r.hp)
-- end