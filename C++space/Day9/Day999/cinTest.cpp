#include<iostream>
#include <string>
#include<vector>
using namespace std;

int main()
{
	vector<string> svec;
	svec.reserve(1024);
	string word;
	while (cin >> word)
	{
		svec.push_back(word);
	}
	svec.resize(svec.size() + svec.size() / 2);
	return 0;
}