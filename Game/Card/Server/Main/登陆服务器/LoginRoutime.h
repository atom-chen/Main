/*

*/

class LoginRoutine :public SupRoutine
{
private:
	ServerSocket m_ServerScoket;
	LoginPlayerManager m_PlayerManager;                //角色管理器（对当前登录玩家进行管理）

private:
	LoginOnlineData m_LoginOnlineData;
	LoginValidataCacheData m_LoginValidataCacheData;
	LoginRoleDataPreloader m_LoginRoleDataPreloader;
private:
	int32_t PlayerCount;                            //当前服务器玩家人数   
	int32_t needInitCount;                         
	int32_t m_ShutdownTime;                         
private:
	int64_t SaveSerial;                            
	int32_t DBDataSaveTime;
	int32_t SMDataSaveTime;                        
                     
	shared_memort_pc sharedMemory;                
private:
	RandomNameManager m_RandomNameMgr;             //随机名称的管理器
	NewRoleLimit m_NewRoleLimit;                   //新角色限制



public:
	explicit LoginRoutine();
	virtual ~LoginRoutine();
public:
	virtual void Open();                           //启服，取数据
	virtual void Shut();                           //停服，数据入库

    //存储与读取数据相关
	void SaveData2DB();
	void SaveData2SM();
	
	bool LoadDataFromSM();
	bool Load(const DB_TranstfirnBewRoleLimit& rMsg);

public:
	bool PlayerCall_OnlineData_CheckAccountState(const Clientplayer &rPlayer);
	bool PlayerCall_OnlineData_Queue(const ClientPlayer& rPlayer);
	void PlayerCall_OnlineData_Offline(const ClientPlayer& rPlayer);

	void AddOfflineCacgeDara(sol_guid userGuid,const OfflineCacheData& data);
	bool GetOfflineCacheData(sol_guid userGuid,OfflineCacheData& data);
	void RemoveOfflineCacheData(sol_guid userGuid);

public:
	//每次Update数据
	virtual void tick(const sol_routine_time &rt); 
	void Tick_ShutDown(const sol_routine_time &rt);
	void Tick_Accept(const sol_routine_time &rt);
	void Tick_Transport(const sol_routine_time &rt);
	void Tick_GlobalPacketStat(const sol_routine_time &rt);
	void Tick_SerPlayerSaveDBInterval(const sol_routine_time &rt);
	
	void Tick_Save(const sol_routine_time& rTimeInfo);


public:
	bool IsHaveNewPlayer();
	bool AcceptNewPlayer();                            
//---------------------------------------------------------------------IDIP----------------------------------------------------------------------
public:
	virtual void Tick_IDIP(const sol_routine_time &rt);
}