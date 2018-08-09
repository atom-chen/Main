echo 准备开始生成表格

set homeford=%cd%
echo %homeford%
cd..
cd..
set workpath=%cd%
cd..
cd..
cd..
set ServerPath=%cd%
cd "%homeford%"

echo 设置目标文件夹
set DstTableFolder=%ServerPath%\Server\Branches\Main\Server\Public\Table\

rem 公共目录下的txt
rem set SrcTableFolder=%workpath%\Public\PublicTables
rem 服务器目录下的txt
set SrcTableFolder=%workpath%\Server\Config

rem 设置本地目录，生成到本地，手动拷贝到指定目标目录
rem set DstTableFolder=./DstCPP
rem set SrcTableFolder=./SrcTXT

rmdir /Q /S CodeTable

echo 请将txt表格放入到%SrcTableFolder%目录下

echo 开始生成代码文件...
if %PROCESSOR_ARCHITECTURE%==x86 goto p1 else goto p2

:p2
echo x64处理中...
PlistTableCode.exe -d:%SrcTableFolder% -C++
goto ep

:p1
echo x86处理中...
PlistTableCode.exe -d:%SrcTableFolder% -C++
goto ep

:ep
echo 拷贝*.cs
copy /Y .\CodeTable\CPlusPlus\*.* "%DstTableFolder%"\
echo 请及时到相关目录Add文件，不然就编不过啦！ 

pause
