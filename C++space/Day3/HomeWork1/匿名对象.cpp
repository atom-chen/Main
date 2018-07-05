#include<iostream>;
using namespace std;

class ABCD{
public:
	ABCD(int a, int b)
	{
		cout << "构造" << endl;
	}
	~ABCD()
	{
		cout << "析构" << endl;

	}
	ABCD(ABCD &obj)
	{
		cout << "拷贝构造" << endl;
	}
private:
	int a;
	int b;
};
int pppp()
{
	
	return 0;
}
int main1()
{
	pppp();
	ABCD(2, 2);
	system("pause");
	return 0;
}