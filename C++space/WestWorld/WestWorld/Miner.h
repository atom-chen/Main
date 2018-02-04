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
//���������
class BaseGameEntity{
protected:
	BaseGameEntity(int id,string name="");
public:
	const string& getName() const;
protected:
	int id;
	string name;
};
//������
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
	//ָ��״̬����ָ��
	StateMachine<Miner> *m_pMachine;
};

//�ڿ�״̬�������л�����ˮ����Ǯ��WC
class EnterMineAndDigForNugget :public State <Miner> {
public:
	virtual void Enter(Miner *pOnwer);
	virtual void Execute(Miner *pOnwer);
	virtual void Exit(Miner *pOnwer);
	static EnterMineAndDigForNugget* Instance();
};

//ȥ���д�Ǯ�������л���WC���ڿ󡢻ؼ�
class VisitBankAndDepositGold :public State < Miner > {
public:
	virtual void Enter(Miner *pOnwer);
	virtual void Execute(Miner *pOnwer);
	virtual void Exit(Miner *pOnwer);
	static VisitBankAndDepositGold* Instance();
};

//�ؼ���Ϣ�������л����ڿ�WC
class GoHomeAndSleepTillRested :public State < Miner > {
public:
	virtual void Enter(Miner *pOnwer);
	virtual void Execute(Miner *pOnwer);
	virtual void Exit(Miner *pOnwer);
	static GoHomeAndSleepTillRested* Instance();
};

//�ڿ�ȥ�ư���� �����л����ڿ�
class QuenchThirst :public State < Miner > {
public:
	virtual void Enter(Miner *pOnwer);
	virtual void Execute(Miner *pOnwer);
	virtual void Exit(Miner *pOnwer);
	static QuenchThirst* Instance();
};

//�ϲ������л���ԭ��״̬
class MinerGlobalState :public State < Miner > {
public:
	virtual void Enter(Miner *pOnwer);
	virtual void Execute(Miner *pOnwer);
	virtual void Exit(Miner *pOnwer);
	static MinerGlobalState* Instance();
};