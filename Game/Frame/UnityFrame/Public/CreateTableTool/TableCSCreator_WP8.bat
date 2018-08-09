echo 准备开始生成表格

set homeford=%cd%
echo %homeford%
cd..
cd..
set workpath=%cd%
cd "%homeford%"

echo 设置目标文件夹
set DstTableFolder=%workpath%\Client\Assets\Plugins\GameTables
set SrcTableFolder=%workpath%\Client\Assets\BundleAssets\Tables

rem 设置本地目录，生成到本地，手动拷贝到指定目标目录
rem set DstTableFolder=./DstCSharp
rem set SrcTableFolder=./SrcTXT

echo 请将txt表格放入到%SrcTableFolder%目录下

echo 开始生成代码文件...

if %PROCESSOR_ARCHITECTURE%==x86 goto p1 else goto p2

:p2
echo x64处理中...
PlistTableCode_WP8.exe -d:%SrcTableFolder% -CSharp
goto ep

:p1
echo x86处理中...
PlistTableCode_WP8.exe -d:%SrcTableFolder% -CSharp
goto ep

:ep
echo 拷贝*.cs
copy /Y .\CodeTable\CSharp\*.cs "%DstTableFolder%"\
echo 请及时到相关目录Add文件，不然就编不过啦！ 

pause
