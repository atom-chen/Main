#include<iostream>
using namespace std;

class gameObject{
public:
	int hp;
	int atk;
	int damage;	
};
class Hero :public gameObject{
public:
	virtual void  atkMonst(gameObject &monst)
	{
		monst.hp = monst.hp-(this->atk - monst.damage);
	}

};
class Yi :public Hero{
public:
	Yi()
	{
		this->hp = 50;
		this->atk = 100;
		this->damage = 0;
	}
	virtual void atkMonst(gameObject &monst)
	{
		cout << "我是易大师，我在偷大龙" << "atk="<<atk<<"hp="<<hp<<endl;
		monst.hp -= (this->atk - monst.damage);
		this->atk+=20;
	}
	
};

class Monst :public gameObject{
public:
	virtual void atkPlayer(Hero &hero)
	{
		hero.hp -= this->atk - hero.damage;
	}
	
};
class Boss :public Monst{
public:
	Boss()
	{
		this->hp = 200;
		this->atk = 10;
		this->damage = 50;	
	}
	void atkPlayer(Hero &hero)
	{
		cout << "我是Boss，我在攻击玩家" << "atk=" << atk << "hp=" << hp << endl;
		hero.hp = hero.hp-(this->atk - hero.damage);
		hero.hp = hero.hp-( this->atk - hero.damage);
	}
};

bool Ground(Hero &hero, Monst &monst)
	{
		while (hero.hp > 0 && monst.hp > 0)
		{
			//玩家攻击
			hero.atkMonst(monst);
			if (monst.hp <= 0)
			{
				break;
			}
			monst.atkPlayer(hero);
		}
		return (hero.hp > 0);
	}


void main()
{
	Yi yi;
	Boss boss;
	bool win = Ground(yi, boss);
	if (win)
	{
		cout << "玩家胜利" << endl;

	}
	else{
		cout << "玩家失败" << endl;
	}
}