function f(a,b)
	print("input",a,b)
	return a or b
end

function foo1() end;

function foo2() 
	return "a" 
end;--返回一个值

function foo3() 
	return "a","b" 
end;--返回两个值

a=f(3);
print(a);

a=f(3,4);
print(a);

a=f(3,4,5);
print(a);

count=0; --全局计数器
function incCount(a)
	n=n or 1 --dafault parameter = 1,防止实参为nil
	count=count+n
end
incCount(9);
print("count=",count);

--多重返回值
s,e=string.find("hello lua!","lua");
print("s=",s,"    e=",e);

a={1,2,3,4,5,9,9,0,2,3,2,-2};
function maxinum(a)
	local maxIndex=1;
	local maxVal=a[maxIndex];
	for i,val in ipairs(a) do
		if val>maxVal then
			maxIndex=i;
			maxVal=val;
		end
	end
	return maxIndex,maxVal;
end;

print("maxinum ret=",maxinum(a));--将打印两个值->此时a是一个table

--若返回值比左值多，则多出来的返回值被丢弃	
a,b,c=10,string.find("hello lua!","lua");
print("s=",a,"e=",b,"c=",c);
x=foo3();--y被丢弃
print("x=foo3(),x=",x);

--当返回值数量不够，则会接到nil
a,b,c,d=10,string.find("hello lua!","lua");		
print("s=",a,"e=",b,"c=",c,"d=",d);

--当函数调用不是最后一个,只产生一个值
x,y=foo3(),20;
print("x,y=foo3(),x=",x,"y=",y);
x,y=foo1(),20,30;--foo1返回nil
print("x,y=foo1(),x=",x,"y=",y);

--若将返回值用作函数调用
print("foo3()=",foo3());--返回值全部传入外部调用（只有一个形参）
print(foo3().."x");--不止一个形参，返回值数量被调整为1