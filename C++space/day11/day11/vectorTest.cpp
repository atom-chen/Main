#include<iostream>
#include <string>
#include<vector>
using namespace std;

void main1()
{
	vector<int> v1;
	v1.reserve(50);
	for (int i = 0; i < 10; i++)
	{
		v1.push_back(i);
		cout << "����"<<i<<"����ʱ="<<v1.capacity()<<"��v1.size()="<<v1.size()<<"  v1��ַΪ"<<
			&v1<<endl;
	}
}