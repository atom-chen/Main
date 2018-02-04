#include<iostream>
#include <string>
using namespace std;


struct pizza{
	string gongsi;
	float zhijing;
	float weight;
};
void print(const pizza obj)
{
	cout<<obj.gongsi<<obj.weight<<obj.zhijing<<endl;
}
