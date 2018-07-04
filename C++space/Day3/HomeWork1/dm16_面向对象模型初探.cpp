#include "iostream"

using namespace std;

class C1
{
public:
	int i;  //4
	int j; //4
	int k;  //4
protected:
private:
}; //12

class C2
{
public:
	int i; 
	int j; 
	int k; 

	static int m; //4
public:
	int getK() const { return k; } //4
	void setK(int val) { k = val; }  //4

protected:
private:
}; //24 16 12(Ìú¶¤µÄ²»¶Ô)

struct S1
{
	int i;
	int j;
	int k;
}; //12

struct S2
{
	int i;
	int j;
	int k;
	static int m;
}; //16

int main()
{
	printf("c1:%d \n", sizeof(C1));
	printf("c2:%d \n", sizeof(C2));
	printf("s1:%d \n", sizeof(S1));
	printf("s2:%d \n", sizeof(S2));

	system("pause");
}
