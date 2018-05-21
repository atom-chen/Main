#pragma  once
#include <iostream>
using namespace std;

class  AbsClass
{
public:
	void Atack();
protected:
	virtual void TestAtack()=0;
	virtual void PlaySound() = 0;
	virtual void PlayEffect() = 0;
private:
};