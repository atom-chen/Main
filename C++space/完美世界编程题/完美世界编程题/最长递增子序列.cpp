#include<iostream>
#include <string>
using namespace std;
//����һ������ΪN�����飬�ҳ�һ����ĵ������������У���һ������������˳�����ң�
int L(int *array, int size)
{
	//����¼����¼��i��Ԫ��Ϊ��ʼ������Ž�
	int *remember = new int[size];
	remember[0] = 0;

	int sub=L(array, size, 0, remember);
	delete[] remember;
	return sub;
}
int& max(int &a, int &b);
int& L(int *array, int size, int i,int *remember)
{
	//�������¼����
	if (remember[i] >= 0)
	{
		return remember[i];
	}
	else{
		//û�У���ȥ��
		//��ǰ��һ����ĳ�����ֿ�ͷ
		//i��ͷ�����Ž�=1+array[i+1,...,size-1]�����Ž�
		for (int j = i; j < size; j++)
		{
			
		}
	}
}
int& max(int &a, int &b)
{
	if (a >= b)
		return a;
	else
		return b;
}
