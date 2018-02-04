#include<iostream>
#include<string>
#include<vector>
using namespace std;

unsigned long seed = 61829450;
double GaussianRand()
{
	double sum = 0;
	for (int i = 0; i < 3; i++)
	{
		unsigned long holdseed = seed;
		//ͨ�������λ��������������������ú����᷵�ط�Χ������[-3.0,3.0]֮�����ֵ
		seed ^= seed << 13;
		seed ^= seed >> 17;
		seed ^= seed << 5;

		long r = (holdseed + seed);
		sum += (double)r*(1.0 / 0x7FFFFFFFFFFFFFFF);
	}
	return sum;
}
int main()
{
	for (int i = 0; i < 50; i++)
	{
		cout << GaussianRand()<<endl;
	}
	return 0;
}