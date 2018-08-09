@echo off

set ClientPath=..\Client\Assets\Game\Lua\Resources\
set ServerPath=..\Server\ScriptBattle\

echo 准备拷贝客户端
echo 删除:%ClientPath%
rd %ClientPath% /s/q
echo 开始拷贝
XCOPY .\LuaScript %ClientPath% /s/I
python ./renameRes.py

echo 准备拷贝服务器
echo 删除:%ServerPath%
rd %ServerPath% /s/q
echo 开始拷贝
XCOPY .\LuaScript %ServerPath% /s/I


echo 生成脚本表格
python ./LuaCodeTools/genScriptTb.py ./LuaScript/ ./PublicTables/BattleScript.txt
echo 生成表格成功

pause