Player={}--基类
Player.__index=Player;

function Player.New()--构造方法
local obj={name="default"}
setmetatable(obj,Player)--在这里，obj变成了Player对象
return obj
end

function Player.__add(a,b)--重载+
print("调用Player.a+b");
return a
end

function Player.__sub(a,b)--重载-
print("调用Player.a-b");
return a
end

function Player.__mul(a,b)--重载*
print("调用Player.a*b");
return a
end

function Player.__div(a,b)--重载/
print("调用Player.a/b");
return a
end

function Player.__unm(a)--重载取负值
print("调用Player.-a");
return a
end

function Player.__pow(a,n)--重载n次幂
print("调用Player.a^n");
return a
end

function Player.__concat(a,n)--重载字符串拼接
print("调用Player.a..n");
return a
end

local player1=Player.New();
local player2=Player.New();

local player3=player1+player2;
player3=player1-player2;
player3=player1*player2;
player3=player1/player2;
player3=-player1;
player3=player1^5;
player3=player1..player2







