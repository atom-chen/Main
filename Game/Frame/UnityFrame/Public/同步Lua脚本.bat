@echo off

set ClientPath=..\Client\Assets\Game\Lua\Resources\
set ServerPath=..\Server\ScriptBattle\

echo ׼�������ͻ���
echo ɾ��:%ClientPath%
rd %ClientPath% /s/q
echo ��ʼ����
XCOPY .\LuaScript %ClientPath% /s/I
python ./renameRes.py

echo ׼������������
echo ɾ��:%ServerPath%
rd %ServerPath% /s/q
echo ��ʼ����
XCOPY .\LuaScript %ServerPath% /s/I


echo ���ɽű����
python ./LuaCodeTools/genScriptTb.py ./LuaScript/ ./PublicTables/BattleScript.txt
echo ���ɱ��ɹ�

pause