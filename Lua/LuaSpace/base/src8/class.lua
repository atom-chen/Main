Player={}--定义一个类
Player.__index=Player;

function Player.New()--类方法
local obj={name="default"}
setmetatable(obj,Player)--在这里，obj变成了Player对象
return obj
end


function Player:Speak()--成员函数
print(self.name.."说话");
end

local player=Player.New();--调用类static函数-》拿到obj
player:Speak();--调用成员函数,相当于obj调用speak方法  但是表里没有Speak方法，然后lua虚拟机就回去查找这个表的metatable（也就是Player）的index方法要这个函数
player.Speak(player)