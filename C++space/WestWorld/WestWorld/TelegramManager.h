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
	void DispatchDelayMessage();//每次检测是否有消息要发送
private:
	//延迟set
	set<Telegram> *DelayPQ=new set<Telegram>;
};