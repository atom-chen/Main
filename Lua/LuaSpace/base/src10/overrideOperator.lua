Player={}--����
Player.__index=Player;

function Player.New()--���췽��
local obj={name="default"}
setmetatable(obj,Player)--�����obj�����Player����
return obj
end

function Player.__add(a,b)--����+
print("����Player.a+b");
return a
end

function Player.__sub(a,b)--����-
print("����Player.a-b");
return a
end

function Player.__mul(a,b)--����*
print("����Player.a*b");
return a
end

function Player.__div(a,b)--����/
print("����Player.a/b");
return a
end

function Player.__unm(a)--����ȡ��ֵ
print("����Player.-a");
return a
end

function Player.__pow(a,n)--����n����
print("����Player.a^n");
return a
end

function Player.__concat(a,n)--�����ַ���ƴ��
print("����Player.a..n");
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







