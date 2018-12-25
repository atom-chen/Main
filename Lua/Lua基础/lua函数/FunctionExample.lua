print(8*9,9/8);--语句
a=math.sin(3)+math.cos(10);--表达式
print(a);

--dofile 'a.lua';
--print[[a multi-linemessage]];

function add(a)
	local sum=0;
	for i,v in ipairs(a) do
		sum=sum+v
	end
	return sum;
end

a={1,2,3,4,5,9,9,0,2,3,2,-2};
print(add(a));