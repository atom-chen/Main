--����t��ÿһ��Ԫ��ֵ�ĵ���
function values(t) 
local i=0 return function() 
i=i+1; return t[i] end end;

t={10,20,30};
iter=values(t);
--ʹ��
--[[
while(true) do
local temp=iter();--ÿִ������ȡ����һ��Ԫ��,��Ϊi�ᱣ��
if(temp==nil) then break end;
print(temp);
end;

for element in values(t) do
print(element);
end
]]



