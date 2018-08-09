@echo off
echo 准备开始拷贝表格

echo 设置目标文件夹

set ServerTableFolder=../Server/Config/

set ClientTableFolder=../Client/Assets/Game/Resources/Bundle/Table/
set PublicTableFolder=./PublicTables/
set SplitTableConfig=SplitTableList.txt
set TempTableFolder=./CreateTableTool/TmpClientDic/

echo 请将公用文件放入到同级PublicTables目录下

echo 检查表格
python ./CreateTableTool/table_checker.py
echo 检查完毕

echo 转化客户端表格和服务器表格为UTF8格式，并拷贝

"./OtherTools/TxtConvExe_Split/txtConv.exe" %TempTableFolder% %PublicTableFolder% %ClientTableFolder% -s 1000 %SplitTableConfig%
"./OtherTools/TxtConvExe_64/TxtConvWin7_64.exe" %PublicTableFolder% %ServerTableFolder%

pause
