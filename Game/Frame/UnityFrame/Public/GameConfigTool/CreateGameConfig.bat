echo ׼����ʼ��������

set homeford=%cd%
echo %homeford%
cd..
cd..

set workpath=%cd%
cd..

set ServerPath=%cd%

cd "%homeford%"

rem ����ini�ļ���
set IniConfigFolder=%workpath%\Server\Config

rem ���ô����ļ���
set DstCppFolder=%ServerPath%\..\..\Server\Branches\Main\Server\Public

rmdir /Q /S tmp
mkdir  tmp\cpp

rem ����Դ�ļ��У����������ɵ�c++��GameConfig.ini
set SrcFolder=%homeford%\tmp


rem ִ��
python CreateGameConfig.py %IniConfigFolder%

copy /Y %SrcFolder%\cpp\*.* "%DstCppFolder%"\Config\

cd "%homeford%"
echo Complete!

pause