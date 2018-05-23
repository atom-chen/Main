Player={}--基类
Player.__index=Player;

function Player.New()--构造方法
local obj={name="default"}
setmetatable(obj,Player)--在这里，obj变成了Player对象
return obj
end

function Player.__eq(a,b)--重载==
print("调用Player.a==b");
return true
end

function Player.__lt(a,b)--重载<
print("调用Player.a<b");
return a
end

function Player.__le(a,b)--重载<=
print("调用Player.a<=b");
return a
end


local player1=Player.New();
local player2=Player.New();

local bool=player1==player2;
bool=player1<player2;
bool=player1<=player2;
bool=player1>player2;
bool=player1>=player2;







