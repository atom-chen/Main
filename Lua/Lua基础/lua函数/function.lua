function foo()
print("foo函数被调用");
end

function add(a,b)
print("add函数被调用");
return a+b;
end

foo();
print(add(1,2));