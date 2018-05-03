--print(8*9,9/8);--语句
a=math.sin(3)+math.cos(10);--表达式
--print(a);

--dofile 'a.lua';
--print[[a multi-linemessage]];

function add(a)
local sum=0;
for i,v in ipairs(a) do
sum=sum+v;
end
return sum;
end;

a={1,2,3,4,5,9,9,0,2,3,2,-2};
--print(add(a));

function f(a,b)
return a or b end;
f(3);f(3,4);f(3,4,5);

count=0;
function incCount(n)
n=n or 1;--or:当第一个为真时返回第一个
count=count+n;
end;
incCount(19);
--print(count);

--多重返回值
s,e=string.find("hello lua!","lua");
--print("开始于"..s.."结束于"..e);

function maxinum(a)
local maxIndex=1;
local maxVal=a[maxIndex];
for i,val in ipairs(a) do
if val>maxVal then
maxIndex=i;
maxVal=val;
end;end;
return maxIndex,maxVal;
end;
--print(maxinum(a));--将打印两个值

function foo1() end;
function foo2() return "a" end;--返回一个值
function foo3() return "a","b" end;--返回两个值
--如果函数调用仅有，则尽可能补充
x,y=foo3()
--print(x,y);
x=foo3();--y被丢弃
--print(x);
x,y,z=10,foo3();
--print(x,y,z);

--用nil补返回值差
x,y=foo1();
--print(x,y);
x,y=foo2();
--print(x,y);

--如果不是最后一个元素，则只产生一个返回值
x,y=foo3(),20;
--print(x,y);
x,y=foo1(),20,30;
--print(x,y);

--print(foo1());
--print(foo2());
--print(foo3());--返回值全部传入外部调用
--print(foo3(),1);
--print(foo3().."x");--出现在表达式中，返回值数量被调整为1

--table构造式接收所有返回值
t1={foo1()};
t2={foo2()};
t3={foo3()};
t4={foo2(),foo3(),4};
for k,v in pairs(t4) do
--print(t4[k]);
end;

--将函数调用放在一对圆括号中，迫使他只返回一个值
--print((foo3()));

--unpack:接收一个数组作为参数，并从下标1开始返回数组所有元素
--print(unpack(t4));
a,b=unpack(t4);
--print(a,b);

f=string.find;
--print(type(f));
a={"hello","c++"};
--print(unpack(a));

function unpack(t,i)
i=i or 1
if t[i] then return t[i],unpack(t,i+1);--只要不为nil或者false就不会产生返回值
end; end;

--变长参数
function add(...)
local s=0;
for i,v in ipairs{...} do--用...访问
s=s+v;
end;
return s;
end;
print(add(1,2,3,4,5,6,1,1,1,1));

function foo(...)
a,b=...;
return a,b;
end;













