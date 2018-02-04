#include<iostream>
#include <string>
using namespace std;
//转化为汉语
class Player{
public:
	virtual void attack()
	{
		cout << "发起进攻"<<endl;
	}
	virtual void defense()
	{
		cout << "防守"<<endl;
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