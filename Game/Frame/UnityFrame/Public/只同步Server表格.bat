@echo off
echo ׼����ʼͬ������˱��

echo ����Ŀ���ļ���
set ServerTableFolder=../Server/Config/
set PublicServerTablesFolder=./ServerTables/

echo �����
python ./CreateTableTool/table_checker.py
echo ������
echo ת�����������ΪUTF8��ʽ��������

if %PROCESSOR_ARCHITECTURE%==x86 goto p1 else goto p2

:p2
echo x64������...
start ./OtherTools/TxtConvExe_64/TxtConvWin7_64.exe  %PublicServerTablesFolder% %ServerTableFolder%
goto ep

:p1
echo x86������...
start ./OtherTools//TxtConvExe/TxtConv.exe  %PublicServerTablesFolder% %ServerTableFolder%
goto ep

:ep
echo �ɹ�

pause
