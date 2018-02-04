#include<iostream>
#include <string>
using namespace std;
//ת��Ϊ����
class Player{
public:
	virtual void attack()
	{
		cout << "�������"<<endl;
	}
	virtual void defense()
	{
		cout << "����"<<endl;
	}
};
class Center
{
public:
	string attack()
	{
		return "attack";
	}
	string defence()
	{
		return "defence";
	}
};
class TransLater :public Player{
public:
	TransLater(Center *center)
	{
		this->center = center;
	}
	virtual void attack()
	{
		center->attack();
		Player::attack();
	}
	virtual void defense()
	{
		center->defence();
		Player::defense();
	}
private:
	Center *center;
};



void main()
{
	Center *center = new Center;
	TransLater translate(center);
	translate.attack();

}