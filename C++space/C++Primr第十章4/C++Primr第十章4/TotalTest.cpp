#include<iostream>
#include<string>
#include<vector>
#include <list>
#include <algorithm>
using namespace std;
void main01()
{
	vector<int> v1 = { 0, 3, 23, 5, 66, 7, 222 };
	auto val=find(v1.begin(), v1.end(), 9);
	if (val != v1.end())
	{
		cout<<*(val)<<endl;
	}
	else{
		cout << "ÕÒ²»µ½"<<endl;
	}
}
void main02()
{
	vector<int> v1 = { 0, 3, 23, 5, 66, 7, 222,3,3,3,3,3,5,8,3 };
	cout<<count(v1.begin(), v1.end(), 3);
}
void main03()
{
	list<string> li;
	while (cin)
	{
		string s = "";
		cin >> s;
		li.push_back(s);
	}
	cout<<count(li.begin(), li.end(), "aaa");
}
void main04()
{

}


int main()
{
	//main01();
	//main02();
	//main03();
	return 0;
}