Player={}--基类
Player.__index=Player;

function Player.New()--类方法
	local obj={name="default"}
setmetatable(obj,Player)--在这里，obj变成了Player对象
return obj
end


function Player:Speak()--成员函数
	print(self.name.."说话");
end


LaoWang={}--
LaoWang.__index=LaoWang;
setmetatable(LaoWang,Player)--让laowang成为Player的子类

function LaoWang.New()--子类初始化函数
	local obj={name="laowang"}
	setmetatable(obj,LaoWang)
	return obj;
end

local laowang=LaoWang.New();
laowang:Speak()











