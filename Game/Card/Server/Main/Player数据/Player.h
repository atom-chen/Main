


class Player
{
public:
	Player();
	virtual ~Player();
protected:
	Socket m_Socket;
private:
	SocketInputStream m_SocketInputStream;
	SocketOutputStream m_SocketOutputStream;

public:
	virtual bool ProcessInput();
	virtual bool ProcessOutput();
	virtual bool ProcessCommand();
	bool ProcessHeartbear(time_t nAnsiTime);
public:
	/*通用消息处理函数*/
}


class ClientPlayer:public Player
{
public:
	ClientPlayer();
	virtual ~ClientPlayer();
public:
	virtual bool ProcessInput();
	virtual bool ProcessOutput();
	virtual bool ProcessCommand();	
private:
	User m_User;
	UserPtr m_UserPtr;
	PlayerLoginData m_PlayerLoginData;
};