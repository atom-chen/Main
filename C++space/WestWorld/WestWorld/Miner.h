#include<iostream>
#include<string>
#include<vector>
#include "StateMachine.h"
using namespace std;

struct position{
	int x;
	int y;
	double rotation;
};
const position MinePos = { 1, 1, 1 };
const position BankPos = { 2, 2, 2 };
const position HomePos = { 3, 3, 3, };
const position BarPos = { 4, 4, 4, };
const position WCPos = { 5, 5, 5, };
//智能体基类
class BaseGameEntity{
protected:
	BaseGameEntity(int id,string name="");
public:
	const string& getName() const;
protected:
	int id;
	string name;
};
//旷工类
class Miner :public BaseGameEntity{
public:
	Miner(int id) :BaseGameEntity(id), m_iGoldCarried(0), m_iMoneyInBank(0), m_iThirst(0), m_iFatigue(0)
	{
		m_pMachine = new StateMachine<Miner>(this);
		m_pMachine->setCurrentState(EnterMineAndDigForNugget::Instance());
		m_pMachine->setGlovalState(MinerGlobalState::Instance());	
	}
	~Miner(){
		if (m_pMachine != nullptr)
		{
			delete m_pMachine;
			m_pMachine = nullptr;
		}	
	}
public:
	void Update();
	void ChangeLocation(const position& newLocation);
	void AddToGoldCarried(const unsigned& gold);
	bool isPocketsFull();
	void SaveToBank();
	void Sleep();
	bool isSleepy();
	void Drink();
	bool isThirsty();
	bool isWanttoWC();
	StateMachine<Miner>& getMachine();
private:
	position m_Location;
	unsigned m_iGoldCarried;
	unsigned m_iMoneyInBank;
	unsigned m_iThirst;
	unsigned m_iFatigue;
	//指向状态机的指针
	StateMachine<Miner> *m_pMachine;
};

//挖矿状态：可以切换到喝水、存钱、WC
class EnterMineAndDigForNugget :public State <Miner> {
public:
	virtual void Enter(Miner *pOnwer);
	virtual void Execute(Miner *pOnwer);
	virtual void Exit(Miner *pOnwer);
	static EnterMineAndDigForNugget* Instance();
};

//去银行存钱，可以切换到WC、挖矿、回家
class VisitBankAndDepositGold :public State < Miner > {
public:
	virtual void Enter(Miner *pOnwer);
	virtual void Execute(Miner *pOnwer);
	virtual void Exit(Miner *pOnwer);
	static VisitBankAndDepositGold* Instance();
};

//回家休息，可以切换到挖矿、WC
class GoHomeAndSleepTillRested :public State < Miner > {
public:
	virtual void Enter(Miner *pOnwer);
	virtual void Execute(Miner *pOnwer);
	virtual void Exit(Miner *pOnwer);
	static GoHomeAndSleepTillRested* Instance();
};

//口渴去酒吧买酒 可以切换到挖矿
class QuenchThirst :public State < Miner > {
public:
	virtual void Enter(Miner *pOnwer);
	virtual void Execute(Miner *pOnwer);
	virtual void Exit(Miner *pOnwer);
	static QuenchThirst* Instance();
};

//上厕所，切换回原先状态
class MinerGlobalState :public State < Miner > {
public:
	virtual void Enter(Miner *pOnwer);
	virtual void Execute(Miner *pOnwer);
	virtual void Exit(Miner *pOnwer);
	static MinerGlobalState* Instance();
};