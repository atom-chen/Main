function f(a,b)
	print("input",a,b)
	return a or b
end

function foo1() end;

function foo2() 
	return "a" 
end;--����һ��ֵ

function foo3() 
	return "a","b" 
end;--��������ֵ

a=f(3);
print(a);

a=f(3,4);
print(a);

a=f(3,4,5);
print(a);

count=0; --ȫ�ּ�����
function incCount(a)
	n=n or 1 --dafault parameter = 1,��ֹʵ��Ϊnil
	count=count+n
end
incCount(9);
print("count=",count);

--���ط���ֵ
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

print("maxinum ret=",maxinum(a));--����ӡ����ֵ->��ʱa��һ��table

--������ֵ����ֵ�࣬�������ķ���ֵ������	
a,b,c=10,string.find("hello lua!","lua");
print("s=",a,"e=",b,"c=",c);
x=foo3();--y������
print("x=foo3(),x=",x);

--������ֵ�������������ӵ�nil
a,b,c,d=10,string.find("hello lua!","lua");		
print("s=",a,"e=",b,"c=",c,"d=",d);

--���������ò������һ��,ֻ����һ��ֵ
x,y=foo3(),20;
print("x,y=foo3(),x=",x,"y=",y);
x,y=foo1(),20,30;--foo1����nil
print("x,y=foo1(),x=",x,"y=",y);

--��������ֵ������������
print("foo3()=",foo3());--����ֵȫ�������ⲿ���ã�ֻ��һ���βΣ�
print(foo3().."x");--��ֹһ���βΣ�����ֵ����������Ϊ1