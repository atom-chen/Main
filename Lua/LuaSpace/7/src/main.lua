--返回t中每一个元素值的迭代
function values(t) 
local i=0 return function() 
i=i+1; return t[i] end end;

t={10,20,30};
iter=values(t);
--使用
--[[
while(true) do
local temp=iter();--每执行依次取出下一个元素,因为i会保留
if(temp==nil) then break end;
print(temp);
end;

for element in values(t) do
print(element);
end
]]



