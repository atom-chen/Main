Player={}--����
Player.__index=Player;

function Player.New()--���췽��
local obj={name="default"}
setmetatable(obj,Player)--�����obj�����Player����
return obj
end

function Player.__eq(a,b)--����==
print("����Player.a==b");
return true
end

function Player.__lt(a,b)--����<
print("����Player.a<b");
return a
end

function Player.__le(a,b)--����<=
print("����Player.a<=b");
return a
end


local player1=Player.New();
local player2=Player.New();

local bool=player1==player2;
bool=player1<player2;
bool=player1<=player2;
bool=player1>player2;
bool=player1>=player2;







