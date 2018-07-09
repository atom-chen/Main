
class LoginRoutine :public SupRoutine
{
private:
	NewRoleLimit m_NewRoleLimit;

	int64_t SaveSerial;
	int32_t DBDataSaveTime;
	int32_t SMDataSaveTime;
	int32_t m_ShutdownTime;
	shared_memort_pc sharedMemory;
private:
	ServerSocket m_ServerScoket;
	LoginPlayerManager m_PlayerManager;
private:
	LoginOnlineData m_LoginOnlineData;
	LoginValidataCacheData m_LoginValidataCacheData;
	LoginRoleDataPreloader m_LoginRoleDataPreloader;
private:
	int32_t PlayerCount;
	
public:
	explicit LoginRoutine();
	virtual ~LoginRoutine();
public:
	virtual int32_t get_id(void) const;
	virtual void init();
	virtual int32_t get_normal_interval();
public:
	virtual void tick(const sol_routine_time &rt);
	virtual void Open();
	virtual void Shut();

public:
	//逻辑线程间交互消息处理函数
	virtual void HandleMsg(const ValidataAccountMsg& rMsg);

}