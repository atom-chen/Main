#include<iostream>
#include<string>
#include<vector>
#include<set>
#include"Telegram.h"
using namespace std;

class TelegramManager{
private:
	TelegramManager();
	TelegramManager& operator=(const TelegramManager&);
	TelegramManager(const TelegramManager&);
public:
	static TelegramManager& Instance();
	bool setMessage(int SenderId, int Received, int MsgType, double DispatchTime=0.0, void *ExtraInfo=nullptr);
	void DispatchDelayMessage();//ÿ�μ���Ƿ�����ϢҪ����
private:
	//�ӳ�set
	set<Telegram> *DelayPQ=new set<Telegram>;
};