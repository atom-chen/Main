#include<iostream>;
using namespace std;

class ABCD{
public:
	ABCD(int a, int b)
	{
		cout << "����" << endl;
	}
	~ABCD()
	{
		cout << "����" << endl;

	}
	ABCD(ABCD &obj)
	{
		cout << "��������" << endl;
	}
private:
	int a;
	int b;
};
int pppp()
{
	
	return 0;
}
int main()
{
	pppp();
	ABCD(2, 2);
	system("pause");
	return 0;
}