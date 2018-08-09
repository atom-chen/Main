
local clamp = function(v,min,max)
    return math.min(math.max(v,min),max)
end

local printf = function(fmt,...)
	print(string.format( fmt,... ))
end

local warn = function(...)
	local t = {...}
	for i=1,#t do
		t[i] = tostring(t[i])
	end
	local msg = string.format( "<color='red'>%s</color>",table.concat( t, "\t"))
	print(debug.traceback(msg,3))
end

local warnf = function(fmt,...)
	local msg = string.format( fmt,... )
	warn(msg)
end

local debuglog = function(...)
	if not _G.debuglogEnable then
		return
	end

	print(...)
end

local mk_warn = function(tag)
	local tagStr = string.format( "[%s]", tag)
	return function(...)
		warn(tagStr,...)
	end
end

local mk_warnf = function(tag)
	return function(fmt,...)
		local str = string.format('[%s]\t%s',tag,string.format( fmt,...))
		warn(str)
	end
end

--条件删除多个元素
--TODO:可优化，倒着删
local removec = function(t,cond)
	local len = #t
	local curIndex = 1
	while curIndex <= len do 
		local v = t[curIndex]
		if cond(v,curIndex) then
			table.remove(t,curIndex)
			len = #t
		else
			curIndex = curIndex + 1
		end
	end
end

local shuffle = function(t,randFunc)
	local count = #t
	for i=1,count-1 do
		local randIndex = randFunc(i+1, count)
		t[i],t[randIndex] = t[randIndex],t[i]
	end
end

--字符串切割
local str_split = function(str,p)
	assert(str and p,'split error')
	local re = {}
	local pstr = str .. p
	local pattern = string.format("(.-)%s",p)
	for s in string.gmatch(pstr,pattern) do
		table.insert(re,s)
	end
	return re
end


local serpent = require("serpent")
local serialize = function(tb)
	return serpent.block(tb,{comment = false,custom = 
		function(tag,head,body,tail)
			--body都是数字，去掉换行符
			if not string.find(body,"%a+") then
				body = string.gsub(body,"[\n%s]", "")
			end
			return string.format('%s%s%s%s',tag,head,body,tail)
		end
	})
end

-- local t = {1,2,3,4,5,6,7}
-- math.randomseed(os.time())
-- shuffle(t,math.random)
-- for _,v in ipairs(t) do
-- 	print(v)
-- end

local function arrayToSet(l)
	local set = {}
	for _,v in ipairs(l) do
		set[v] = true
	end
	return set
end

return {
	printf = printf,
	warn = warn,
	warnf = warnf,
    clamp = clamp,
    removec = removec,
	serialize = serialize,
	mk_warn = mk_warn,
	mk_warnf = mk_warnf,
	str_split = str_split,
	shuffle = shuffle,
	arrayToSet = arrayToSet,
	debuglog = debuglog,
}