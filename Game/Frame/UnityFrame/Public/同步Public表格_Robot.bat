@echo off
echo ׼����ʼ�������

echo ����Ŀ���ļ���
set ClientTableFolder=../Robot\Assets\Robot\Resources\Bundle\Table/
set PublicClientFolder=./ClientTables/
set ServerTableFolder=../Server/Config/
set PublicTableFolder=./PublicTables/

echo �뽫�����ļ����뵽ͬ��PublicTablesĿ¼��
echo �����
python ./CreateTableTool/table_checker.py
echo ������

echo ת���ͻ��˱��ͷ��������ΪUTF8��ʽ��������

if %PROCESSOR_ARCHITECTURE%==x86 goto p1 else goto p2

:p2
echo x64������...
start ./OtherTools/TxtConvExe_64/TxtConvWin7_64.exe   %PublicTableFolder% %ClientTableFolder%
start ./OtherTools/TxtConvExe_64/TxtConvWin7_64.exe   %PublicClientFolder% %ClientTableFolder%
start ./OtherTools/TxtConvExe_64/TxtConvWin7_64.exe   %PublicTableFolder% %ServerTableFolder%

goto ep

:p1
echo x86������...
start ./OtherTools//TxtConvExe/TxtConv.exe  %PublicTableFolder% %ClientTableFolder%
start ./OtherTools//TxtConvExe/TxtConv.exe  %PublicClientFolder% %ClientTableFolder%
start ./OtherTools//TxtConvExe/TxtConv.exe  %PublicTableFolder% %ServerTableFolder%
goto ep

:ep
echo �ɹ�

pause
