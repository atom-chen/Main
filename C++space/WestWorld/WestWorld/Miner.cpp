#include<iostream>
#include<string>
#include<vector>
#include<time.h>
#include "Miner.h"
using namespace std;

void Miner::Update()
{
	if (m_pMachine != nullptr)
	{
		this->m_pMachine->Update();
	}
}
void Miner::ChangeLocation(const position& newLocation)
{
	this->m_Location=newLocation;
	cout << "Bob移动到"<<endl;
}
void Miner::AddToGoldCarried(const unsigned& gold)
{
	this->m_iGoldCarried += gold;
}
void Miner::SaveToBank()
{
	this->m_iMoneyInBank += this->m_iGoldCarried;
	this->m_iGoldCarried = 0;
	cout << "Bob将钱全部存到银行"<<endl;
}
bool Miner::isPocketsFull()
{
	return this->m_iGoldCarried >= 100;
}
void Miner::Sleep()
{
	this->m_iThirst--;
	cout << "Bob在家睡觉"<<endl;
}
bool Miner::isSleepy()
{
	return this->m_iFatigue >= 100;
}
void Miner::Drink()
{
	this->m_iFatigue--;
	cout << "Bob在酒吧喝牛奶"<<endl;
}
bool Miner::isThirsty()
{
	return this->m_iThirst >= 100;
}
bool Miner::isWanttoWC()
{

}
StateMachine<Miner>& Miner::getMachine()
{
	return *(this->m_pMachine);
}
//挖矿
void EnterMineAndDigForNugget::Enter(Miner *pOnwer)
{
	pOnwer->ChangeLocation(MinePos);
	cout << "Bob移动到金矿"<<endl;
}
void EnterMineAndDigForNugget::Execute(Miner *pOnwer)
{
	//挖矿中
	pOnwer->AddToGoldCarried(1);
	cout << "Bob正在挖矿"<<endl;
	//如果钱够了
	if (pOnwer->isPocketsFull())
	{
		pOnwer->getMachine().ChangeState(VisitBankAndDepositGold::Instance());
		return;
	}
    //如果渴了
	if (pOnwer->isThirsty())
	{
		pOnwer->getMachine().ChangeState(QuenchThirst::Instance());
		return;
	}
	//去过想去厕所
	if (pOnwer->isWanttoWC())
	{
		pOnwer->getMachine().ChangeState(MinerGlobalState::Instance());
		return;
	}
	
}
void EnterMineAndDigForNugget::Exit(Miner *pOnwer){
	//Bob离开金矿，清理资源
	cout << "Bob离开金矿"<<endl;
}
EnterMineAndDigForNugget* EnterMineAndDigForNugget::Instance()
{
	static EnterMineAndDigForNugget instance;
	return &instance;
}
//去银行存钱
void VisitBankAndDepositGold::Enter(Miner *pOnwer)
{
	pOnwer->ChangeLocation(BankPos);
	cout << "Bob来到银行"<<endl;
}
void VisitBankAndDepositGold::Execute(Miner *pOnwer)
{
	pOnwer->SaveToBank();
	cout << "Bob去银行存钱"<<endl;
	
}
void VisitBankAndDepositGold::Exit(Miner *pOnwer)
{
	cout << "Bob离开银行"<<endl;
}
static VisitBankAndDepositGold* Instance()
{
	static VisitBankAndDepositGold instance;
	return &instance;
}
//回家睡觉
void GoHomeAndSleepTillRested::Enter(Miner *pOnwer)
{
	//移动到家里
	pOnwer->ChangeLocation(HomePos);
	cout << "Bob移动到家里"<<endl;
}
void GoHomeAndSleepTillRested::Execute(Miner *pOnwer)
{
	//正在睡觉
	pOnwer->Sleep();
	cout << "Zzz..."<<endl;
}
void GoHomeAndSleepTillRested::Exit(Miner *pOnwer)
{
	//起床
	cout << "Bob起床了"<<endl;
}
GoHomeAndSleepTillRested* GoHomeAndSleepTillRested::Instance()
{
	static GoHomeAndSleepTillRested instance;
	return (&instance);
}
//口渴去买酒
void QuenchThirst::Enter(Miner *pOnwer)
{
	//移动到酒吧
	pOnwer->ChangeLocation(BarPos);
	cout << "Bob移动到酒吧"<<endl;
}
void QuenchThirst::Execute(Miner *pOnwer)
{
	//喝酒
	pOnwer->Drink();
	cout << "Bob开始喝酒"<<endl;
}
void QuenchThirst::Exit(Miner *pOnwer)
{
	//离开酒吧
	cout << "Bob离开酒吧"<<endl;
}
QuenchThirst* QuenchThirst::Instance()
{
	static QuenchThirst instance;
	return &instance;
}
//上厕所
void MinerGlobalState::Enter(Miner *pOnwer)
{
	pOnwer->ChangeLocation(WCPos);
	cout <<pOnwer->getName() <<"移动到厕所"<<endl;
}
void MinerGlobalState::Execute(Miner *pOnwer)
{
	cout << "上厕所"<<endl;
	//切换回原先状态
	pOnwer->getMachine().RevertToPreviousState();
}
void MinerGlobalState::Exit(Miner *pOnwer)
{
	cout << "离开厕所"<<endl;
}
MinerGlobalState* MinerGlobalState::Instance()
{
	static MinerGlobalState instance;
	return &instance;
}