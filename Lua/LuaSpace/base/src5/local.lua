a=1 --全局a
local b=2--这个是local.lua文件里的b

function New()
local obj={name="faker",age=22,isFriane=false}  --New函数这是局部变量
a=3;
b=4;
return obj;
end

entity=New();

--obj.name="waht" --访问失败
print(a)