@echo off
echo ׼����ʼͬ���ͻ��˱��

echo ����Ŀ���ļ���
set ClientTableFolder=../Robot\Assets\Robot\Resources\Bundle\Table/
set PublicClientFolder=./ClientTables/

echo �뽫�����ļ����뵽ͬ��ClientTablesĿ¼��

echo �����
python ./CreateTableTool/table_checker.py
echo ������
echo ת���ͻ���txtΪUTF8��ʽ��������

if %PROCESSOR_ARCHITECTURE%==x86 goto p1 else goto p2

:p2
echo x64������...
start ./OtherTools/TxtConvExe_64/TxtConvWin7_64.exe   %PublicClientFolder% %ClientTableFolder%
goto ep

:p1
echo x86������...
start ./OtherTools//TxtConvExe/TxtConv.exe  %PublicClientFolder% %ClientTableFolder%
goto ep

:ep
echo �ɹ�

pause