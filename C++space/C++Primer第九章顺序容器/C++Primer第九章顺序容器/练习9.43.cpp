#include<iostream>
#include<string>
#include<vector>
using namespace std;
/*
bool replace_String(string &s, const string& oldVal, const string& newVal);
int main()
{
	string s("woaic++");
	string oldVal("c++");
	string newVal("guoyue");
	replace_String(s, oldVal, newVal);
	cout<<s<<endl;
	return 0;
}
bool replace_String(string &s,const string& oldVal, const string& newVal)
{
	unsigned pos = 0;
	while (pos<s.size())
	{
		//寻找下一个oldVal
		pos=s.find(oldVal, pos);
		if (pos == -1 || pos >= s.size())
		{
			break;
		}
		//替换
		s.erase(pos, oldVal.size());
		//插入
		s.insert(pos,newVal);
		pos++;
	}
	return true;
	
}
*/