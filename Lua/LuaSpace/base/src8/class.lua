Player={}--����һ����
Player.__index=Player;

function Player.New()--�෽��
local obj={name="default"}
setmetatable(obj,Player)--�����obj�����Player����
return obj
end


function Player:Speak()--��Ա����
print(self.name.."˵��");
end

local player=Player.New();--������static����-���õ�obj
player:Speak();--���ó�Ա����,�൱��obj����speak����  ���Ǳ���û��Speak������Ȼ��lua������ͻ�ȥ����������metatable��Ҳ����Player����index����Ҫ�������
player.Speak(player)