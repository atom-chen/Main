require("class")
local Random = class("Random")

local base = 10000000
local a = 214013
local c = 231011

local function rand(seed)
	--应该是直接位移
	return (seed * a + c) % base
end

function Random:Init(seed)
	seed = seed or 214013
	self.seed = seed % base
end

function Random:Rand(min,max)
	local seed = rand(self.seed)

	local isInteger = true 
	if min == nil and max == nil then
		max = 1
		min = 0
		isInteger = false
	elseif max == nil then
			max = min
			min = 1
	end
	local t = min + ((max - min) * seed ) / base
	if isInteger then
	 	t = math.floor(t + 0.5)
	end
	self.seed = seed
	return t 
end

-- local r = new(Random)
-- r:Init(10001)
-- for i=1,1000 do
--     print(r:Rand(1,100))
-- end

return Random