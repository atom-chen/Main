@echo off
echo 准备开始同步客户端表格

echo 设置目标文件夹
set ClientTableFolder=../Robot\Assets\Robot\Resources\Bundle\Table/
set PublicClientFolder=./ClientTables/

echo 请将公用文件放入到同级ClientTables目录下

echo 检查表格
python ./CreateTableTool/table_checker.py
echo 检查完毕
echo 转化客户端txt为UTF8格式，并拷贝

if %PROCESSOR_ARCHITECTURE%==x86 goto p1 else goto p2

:p2
echo x64处理中...
start ./OtherTools/TxtConvExe_64/TxtConvWin7_64.exe   %PublicClientFolder% %ClientTableFolder%
goto ep

:p1
echo x86处理中...
start ./OtherTools//TxtConvExe/TxtConv.exe  %PublicClientFolder% %ClientTableFolder%
goto ep

:ep
echo 成功

pause