--[[a={p=print};
print(type(a));
print(type(a.p));
a.p("hello world!"); --a.p引用a

print=math.sin;
a.p(print(1));
sin=a.p;
sin(10,20);
]]
function MyPrint(network)
for k,v in pairs(network) do
print(network[k].name,network[k].IP);
end;
end;

network={{name="grauna",IP="210.26.30.34"},{name="arraial",IP="210.26.30.23"},{name="lua",IP="210.26.23.12"},
{name="derain",IP="210.26.23.20"}};
--MyPrint(network);
--print("排序后")
table.sort(network,function(a,b) return (a.name>b.name) end);
--MyPrint(network);
function boolean(a,b) return (a>b) end;
--print(boo(10,20));

function derivative(f,delta)
delta=delta or 1e-4;
return function(x) return (f(x+delta)-f(x))/delta end end;

c=derivative(math.sin);
--print(math.cos(10),c(10));


--闭合函数
--需求：按年级来对姓名排序
names={"Peter","Paul","Mary"};--数组
grade={Mary=10,Paul=7,Peter=8};--Mary是key,10是value
--table.sort(names,function(a,b) return grade[a]>grade[b] end);--如果年级a大于年级b 将交换姓名的位置
--print(names[3]);
--print(grade["Mary"]);

function sortByGrade(names,grade)
table.sort(names,function(a,b) return (grade[a]>grade[b]) end);
end;

sortByGrade(names,grade);
--print(names[3]);
--print(grade["Mary"]);

function newCounter()
local i=0;
return function() i=i+1 return i end end;
c1=newCounter();

--print(c1());
--print(c1());

do local odlSin=math.sin
local k=math.pi/180;
math.sin=function(x) return oldSin(x*k) end end;

--非全局的函数
Lib={};
Lib.foo=function(x,y) return x+y end;
Lib.goo=function(x,y) return x-y end;









