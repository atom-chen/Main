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
	cout << "Bob�ƶ���"<<endl;
}
void Miner::AddToGoldCarried(const unsigned& gold)
{
	this->m_iGoldCarried += gold;
}
void Miner::SaveToBank()
{
	this->m_iMoneyInBank += this->m_iGoldCarried;
	this->m_iGoldCarried = 0;
	cout << "Bob��Ǯȫ���浽����"<<endl;
}
bool Miner::isPocketsFull()
{
	return this->m_iGoldCarried >= 100;
}
void Miner::Sleep()
{
	this->m_iThirst--;
	cout << "Bob�ڼ�˯��"<<endl;
}
bool Miner::isSleepy()
{
	return this->m_iFatigue >= 100;
}
void Miner::Drink()
{
	this->m_iFatigue--;
	cout << "Bob�ھưɺ�ţ��"<<endl;
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
//�ڿ�
void EnterMineAndDigForNugget::Enter(Miner *pOnwer)
{
	pOnwer->ChangeLocation(MinePos);
	cout << "Bob�ƶ������"<<endl;
}
void EnterMineAndDigForNugget::Execute(Miner *pOnwer)
{
	//�ڿ���
	pOnwer->AddToGoldCarried(1);
	cout << "Bob�����ڿ�"<<endl;
	//���Ǯ����
	if (pOnwer->isPocketsFull())
	{
		pOnwer->getMachine().ChangeState(VisitBankAndDepositGold::Instance());
		return;
	}
    //�������
	if (pOnwer->isThirsty())
	{
		pOnwer->getMachine().ChangeState(QuenchThirst::Instance());
		return;
	}
	//ȥ����ȥ����
	if (pOnwer->isWanttoWC())
	{
		pOnwer->getMachine().ChangeState(MinerGlobalState::Instance());
		return;
	}
	
}
void EnterMineAndDigForNugget::Exit(Miner *pOnwer){
	//Bob�뿪���������Դ
	cout << "Bob�뿪���"<<endl;
}
EnterMineAndDigForNugget* EnterMineAndDigForNugget::Instance()
{
	static EnterMineAndDigForNugget instance;
	return &instance;
}
//ȥ���д�Ǯ
void VisitBankAndDepositGold::Enter(Miner *pOnwer)
{
	pOnwer->ChangeLocation(BankPos);
	cout << "Bob��������"<<endl;
}
void VisitBankAndDepositGold::Execute(Miner *pOnwer)
{
	pOnwer->SaveToBank();
	cout << "Bobȥ���д�Ǯ"<<endl;
	
}
void VisitBankAndDepositGold::Exit(Miner *pOnwer)
{
	cout << "Bob�뿪����"<<endl;
}
static VisitBankAndDepositGold* Instance()
{
	static VisitBankAndDepositGold instance;
	return &instance;
}
//�ؼ�˯��
void GoHomeAndSleepTillRested::Enter(Miner *pOnwer)
{
	//�ƶ�������
	pOnwer->ChangeLocation(HomePos);
	cout << "Bob�ƶ�������"<<endl;
}
void GoHomeAndSleepTillRested::Execute(Miner *pOnwer)
{
	//����˯��
	pOnwer->Sleep();
	cout << "Zzz..."<<endl;
}
void GoHomeAndSleepTillRested::Exit(Miner *pOnwer)
{
	//��
	cout << "Bob����"<<endl;
}
GoHomeAndSleepTillRested* GoHomeAndSleepTillRested::Instance()
{
	static GoHomeAndSleepTillRested instance;
	return (&instance);
}
//�ڿ�ȥ���
void QuenchThirst::Enter(Miner *pOnwer)
{
	//�ƶ����ư�
	pOnwer->ChangeLocation(BarPos);
	cout << "Bob�ƶ����ư�"<<endl;
}
void QuenchThirst::Execute(Miner *pOnwer)
{
	//�Ⱦ�
	pOnwer->Drink();
	cout << "Bob��ʼ�Ⱦ�"<<endl;
}
void QuenchThirst::Exit(Miner *pOnwer)
{
	//�뿪�ư�
	cout << "Bob�뿪�ư�"<<endl;
}
QuenchThirst* QuenchThirst::Instance()
{
	static QuenchThirst instance;
	return &instance;
}
//�ϲ���
void MinerGlobalState::Enter(Miner *pOnwer)
{
	pOnwer->ChangeLocation(WCPos);
	cout <<pOnwer->getName() <<"�ƶ�������"<<endl;
}
void MinerGlobalState::Execute(Miner *pOnwer)
{
	cout << "�ϲ���"<<endl;
	//�л���ԭ��״̬
	pOnwer->getMachine().RevertToPreviousState();
}
void MinerGlobalState::Exit(Miner *pOnwer)
{
	cout << "�뿪����"<<endl;
}
MinerGlobalState* MinerGlobalState::Instance()
{
	static MinerGlobalState instance;
	return &instance;
}