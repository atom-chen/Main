Player={}--����
Player.__index=Player;

function Player.New()--�෽��
	local obj={name="default"}
setmetatable(obj,Player)--�����obj�����Player����
return obj
end


function Player:Speak()--��Ա����
	print(self.name.."˵��");
end


LaoWang={}--
LaoWang.__index=LaoWang;
setmetatable(LaoWang,Player)--��laowang��ΪPlayer������

function LaoWang.New()--�����ʼ������
	local obj={name="laowang"}
	setmetatable(obj,LaoWang)
	return obj;
end

local laowang=LaoWang.New();
laowang:Speak()











