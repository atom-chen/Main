--print(8*9,9/8);--���
a=math.sin(3)+math.cos(10);--���ʽ
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
n=n or 1;--or:����һ��Ϊ��ʱ���ص�һ��
count=count+n;
end;
incCount(19);
--print(count);

--���ط���ֵ
s,e=string.find("hello lua!","lua");
--print("��ʼ��"..s.."������"..e);

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
--print(maxinum(a));--����ӡ����ֵ

function foo1() end;
function foo2() return "a" end;--����һ��ֵ
function foo3() return "a","b" end;--��������ֵ
--����������ý��У��򾡿��ܲ���
x,y=foo3()
--print(x,y);
x=foo3();--y������
--print(x);
x,y,z=10,foo3();
--print(x,y,z);

--��nil������ֵ��
x,y=foo1();
--print(x,y);
x,y=foo2();
--print(x,y);

--����������һ��Ԫ�أ���ֻ����һ������ֵ
x,y=foo3(),20;
--print(x,y);
x,y=foo1(),20,30;
--print(x,y);

--print(foo1());
--print(foo2());
--print(foo3());--����ֵȫ�������ⲿ����
--print(foo3(),1);
--print(foo3().."x");--�����ڱ��ʽ�У�����ֵ����������Ϊ1

--table����ʽ�������з���ֵ
t1={foo1()};
t2={foo2()};
t3={foo3()};
t4={foo2(),foo3(),4};
for k,v in pairs(t4) do
--print(t4[k]);
end;

--���������÷���һ��Բ�����У���ʹ��ֻ����һ��ֵ
--print((foo3()));

--unpack:����һ��������Ϊ�����������±�1��ʼ������������Ԫ��
--print(unpack(t4));
a,b=unpack(t4);
--print(a,b);

f=string.find;
--print(type(f));
a={"hello","c++"};
--print(unpack(a));

function unpack(t,i)
i=i or 1
if t[i] then return t[i],unpack(t,i+1);--ֻҪ��Ϊnil����false�Ͳ����������ֵ
end; end;

--�䳤����
function add(...)
local s=0;
for i,v in ipairs{...} do--��...����
s=s+v;
end;
return s;
end;
print(add(1,2,3,4,5,6,1,1,1,1));

function foo(...)
a,b=...;
return a,b;
end;













