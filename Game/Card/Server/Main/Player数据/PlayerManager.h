


class Player
{
private:
	int32_t id;
	bool bKick;
	SocketInputStream socketInputStream;
	SocketOutputStream socketOutputStream;
	Socket socket;
	PacketStat packetStat;

protected:
	int32_t status;        //当前状态

public:
	/*  所有消息处理函数 */

}

using PlayerPtr = boost::shared_ptr<Player>;
using PlayerPtrList = std::list<PlayerPtr>;
class PlayerManager
{
public:
	void Init();
	void Tick(const sol_routine_time& rt);          //相当于服务器的一次Update        
public:
	//每次tick会被处理的函数
	void QuerySockets();
	void ProcessInputs();
	void ProcessOutputs();
	void ProcessExceptions();
	void ProcessCommands();
	void ProcessTicks(const sol_routine_time &rt);
	void ProcessHeartbeat(const sol_routine_time &rt);
protected:
	void QueryConnectingSockets();
	void ProcessConnecting();

private:
	//容器操作函数
	void Add(PlayerPtr ptr,int32_t nResult);           //操作playerList集合，从集合中增加该玩家数据，result为删除的原因
	iterator Del(PlayerPtr ptr,int32_t nResult);       //操作playerList集合，从集合中删除该玩家数据，result为删除的原因
protected:
	PlayerPtrList m_PlayerList;                    //核心容器，存储当前在线所有玩家
	PlayerPtrList m_ConnectingPlayerList;          //存储当前已建立的连接
protected:
	//全部set
	fd_set m_ReadSet;
	fd_set m_WriteSet;
	fd_set m_ExceptSet;

	fd_set m_ConnectingReadSet;
	fd_set m_ConnectingWriteSet;
	fd_set m_ConnectingExceptSet;
public:
	//预留接口
	virtual void OnAddPlayer(PlayerPtr ptr,int32_t nResult);
	virtual void OnDelPlayer(PlayerPtr ptr,int32_t nResult);
	virtual void OnAddConnectingPlayer(PlayerPtr ptr,int32_t nResult);
	virtual void OnDelConnectingPlayer(PlayerPtr ptr,int32_t nResult);		
}



class LoginPlayerManager:public PlayerManager
{
public:
	LoginPlayerManager();
	~LoginPlayerManager();
public:
	virtual void OnAddPlayer(PlayerPtr ptr,int32_t nResult);
	virtual void OnDelPlayer(PlayerPtr ptr,int32_t nResult);	
private:
	LoginRoutine& m_rLoginRoutine;
};