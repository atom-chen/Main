#include<iostream>
#include <string>
using namespace std;

void main()
{
	for (int i = 0; i < 10; i++)
	{
		int j = static_cast<int>(100.0*rand() / (RAND_MAX + 1));
		cout << j << endl;
	}

}