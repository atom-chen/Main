local cat=Cat.New();
cat:Eat();

CFunc();--调用C写入的函数

CFunc2("hello,I came from lua!",1);--调用c写入的函数

local a=CFunc3();--调用c写入的函数
print(a);


print(Alice.RunTimePlatform())
