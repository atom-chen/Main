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

--局部变量
x=10;
local i=1;--i虽然是局部变量，但是生命周期还没结束
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

--显式控制程序块
do
local a2=2*a;
local d=(b^2-4*a*2)^(1/2);
x1=(-b+d)/a2;
x2=(-b-d)/a2;
end;
--print(a2,d);--此时a2,d生命周期已经结束
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

--控制结构
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
until line~=""--为真时结束
print(line);
]]

local sqr=x/2
repeat
	sqr=(sqr+x/sqr)/2;
	local error=math.abs(sqr^2-x);
until error<x/10000;--仍然可以访问local error

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
--print(max); 此时max=6 是全局变量i

--找到a中第一个小于0的数的下标
a={1,2,3,4,5,9,9,0,2,3,2,-2};
local found=nil;
for i=1,#a do
	if a[i]<0 then
		found=i;break;
		end;end;
--print("a["..found.."]="..a[found]);

--泛型for
--ipairs()：遍历数组
for i,v in ipairs(a)do 
--print(v)
end;

--table元素的pairs
revDays={["sunday"]=1,["Monday"]=2,["Tuesday"]=3,["Wednesday"]=4,["Thursday"]=5,["Friday"]=6,["Saturday"]=7};
for k,v in pairs(revDays)
	do  print(revDays[k]);
end








