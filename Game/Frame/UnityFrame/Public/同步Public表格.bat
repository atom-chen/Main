@echo off
echo ׼����ʼ�������

echo ����Ŀ���ļ���

set ServerTableFolder=../Server/Config/

set ClientTableFolder=../Client/Assets/Game/Resources/Bundle/Table/
set PublicTableFolder=./PublicTables/
set SplitTableConfig=SplitTableList.txt
set TempTableFolder=./CreateTableTool/TmpClientDic/

echo �뽫�����ļ����뵽ͬ��PublicTablesĿ¼��

echo �����
python ./CreateTableTool/table_checker.py
echo ������

echo ת���ͻ��˱��ͷ��������ΪUTF8��ʽ��������

"./OtherTools/TxtConvExe_Split/txtConv.exe" %TempTableFolder% %PublicTableFolder% %ClientTableFolder% -s 1000 %SplitTableConfig%
"./OtherTools/TxtConvExe_64/TxtConvWin7_64.exe" %PublicTableFolder% %ServerTableFolder%

pause
