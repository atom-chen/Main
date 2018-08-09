echo 准备开始生成配置

set homeford=%cd%
echo %homeford%
cd..
cd..

set workpath=%cd%
cd..

set ServerPath=%cd%

cd "%homeford%"

rem 设置ini文件夹
set IniConfigFolder=%workpath%\Server\Config

rem 设置代码文件夹
set DstCppFolder=%ServerPath%\..\..\Server\Branches\Main\Server\Public

rmdir /Q /S tmp
mkdir  tmp\cpp

rem 设置源文件夹，里面有生成的c++和GameConfig.ini
set SrcFolder=%homeford%\tmp


rem 执行
python CreateGameConfig.py %IniConfigFolder%

copy /Y %SrcFolder%\cpp\*.* "%DstCppFolder%"\Config\

cd "%homeford%"
echo Complete!

pause