--=
a="hello".."world";
b,c=10,2*2;
--print(a,b,c);

a,b=b,a;
--print(a,b);
a,b,c=0;
--print(a,b,c);
a,b=10,20,30;
--print(a,b);

--�ֲ�����
x=10;
local i=1;--i��Ȼ�Ǿֲ������������������ڻ�û����
while i<=10 do
local x=i*2;
--print(x);
i=i+1;
end;

if i>20 then
local x=20;
--print(x+2);
else
--print(x);
end;

--��ʽ���Ƴ����
do
local a2=2*a;
local d=(b^2-4*a*2)^(1/2);
x1=(-b+d)/a2;
x2=(-b-d)/a2;
end;
--print(a2,d);--��ʱa2,d���������Ѿ�����
--print(x1,x2);
--print(i);

local a,b=1,10;
--print(a,b);
if a<b then
--print(a);
local a;
--print(a);
end;
--print(a,b);

--���ƽṹ
if a<0 then
a=0;
end;
--print(a);

--[[if(a<b) then 
return a 
else return b 
end;]]

local op="+";
local r=0;
if op=="+" then
r=a+b;
elseif op=="-" then
r=a-b;
elseif op=="*" then
r=a*b;
elseif op=="/" then
r=a/b;
else error("???");
end
--print("r="..r);

a={1,2,3,4,5};
--while
local i=1;
while a[i] do
--print(a[i])
i=i+1;
end;

--[[
repeat
line=io.read();
until line~=""--Ϊ��ʱ����
print(line);
]]

local sqr=x/2
repeat
sqr=(sqr+x/sqr)/2;
local error=math.abs(sqr^2-x);
until error<x/10000;--��Ȼ���Է���local error

--for
for i=1,10 do 
--print(i)
end

for i=1,math.huge do
if 0.3*i^3-20*i^2 -500>=0 then
--print(i);
break;
end
end

for i=1,10 do --print(i) 
end;
max=i;
--print(max); ��ʱmax=6 ��ȫ�ֱ���i

--�ҵ�a�е�һ��С��0�������±�
a={1,2,3,4,5,9,9,0,2,3,2,-2};
local found=nil;
for i=1,#a do
if a[i]<0 then
found=i;break;
end;end;
--print("a["..found.."]="..a[found]);

--����for
--ipairs()����������
for i,v in ipairs(a)do 
--print(v)
end;

--tableԪ�ص�pairs
revDays={["sunday"]=1,["Monday"]=2,["Tuesday"]=3,["Wednesday"]=4,["Thursday"]=5,["Friday"]=6,["Saturday"]=7};
for k,v in pairs(revDays)
do  print(revDays[k]);
end








