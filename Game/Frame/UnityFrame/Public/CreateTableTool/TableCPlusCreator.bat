echo ׼����ʼ���ɱ��

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

echo ����Ŀ���ļ���
set DstTableFolder=%ServerPath%\Server\Branches\Main\Server\Public\Table\

rem ����Ŀ¼�µ�txt
rem set SrcTableFolder=%workpath%\Public\PublicTables
rem ������Ŀ¼�µ�txt
set SrcTableFolder=%workpath%\Server\Config

rem ���ñ���Ŀ¼�����ɵ����أ��ֶ�������ָ��Ŀ��Ŀ¼
rem set DstTableFolder=./DstCPP
rem set SrcTableFolder=./SrcTXT

rmdir /Q /S CodeTable

echo �뽫txt�����뵽%SrcTableFolder%Ŀ¼��

echo ��ʼ���ɴ����ļ�...
if %PROCESSOR_ARCHITECTURE%==x86 goto p1 else goto p2

:p2
echo x64������...
PlistTableCode.exe -d:%SrcTableFolder% -C++
goto ep

:p1
echo x86������...
PlistTableCode.exe -d:%SrcTableFolder% -C++
goto ep

:ep
echo ����*.cs
copy /Y .\CodeTable\CPlusPlus\*.* "%DstTableFolder%"\
echo �뼰ʱ�����Ŀ¼Add�ļ�����Ȼ�ͱ಻������ 

pause
