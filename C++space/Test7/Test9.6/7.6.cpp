#include<iostream>
using namespace std;

void Fill_array(double *arr, int length)
{
	int count=0;
	for (int i = 0; i < length; i++)
	{
		cout << "����������" << endl;
		cin >> arr[i];
		count++;
	}
	//��ӡ����
	for (int i = 0; i < count; i++)
	{
		cout << arr[i] << " ";
	}
}

void main()
{

}