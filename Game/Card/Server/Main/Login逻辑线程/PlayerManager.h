


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

protected:
	PlayerPtrList m_PlayerList;                    //核心容器，存储当前在线所有玩家
	PlayerPtrList m_ConnectingPlayerList;          //存储当前已建立的连接
}