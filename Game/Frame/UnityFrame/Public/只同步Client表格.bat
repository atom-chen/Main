@echo off
echo ׼����ʼͬ���ͻ��˱��

echo ����Ŀ���ļ���
set TempTableFolder=./CreateTableTool/TmpClientDic/
set ClientTableFolder=../Client/Assets/Game/Resources/Bundle/Table/
set PublicClientFolder=./ClientTables/

echo �뽫�����ļ����뵽ͬ��ClientTablesĿ¼��

echo �����
python ./CreateTableTool/table_checker.py
echo ������

echo ת���ͻ���txtΪUTF8��ʽ��������

"./OtherTools/TxtConvExe_Split/txtConv.exe" %TempTableFolder% %PublicClientFolder% %ClientTableFolder% -s 1000 SplitTableList.txt


pause