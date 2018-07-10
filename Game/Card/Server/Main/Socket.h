#if  defined(WINDOWS)
     #include <WinSock2.h>
     #include <WS2tcpip.h>
#elif defined(LINUX)
     #include <netinet/in.h>
     #include <arpa/inet.h>
     #include <sys/types.h>
     #include <sys/spcket.h>
     #include <poll.h>

#defined IP_SIZE 24

class Socket
{
public:
	Socket();
	virtual ~Socket();
public:
	//创建 销毁连接
	bool create();
	void close();

	bool connect();
	bool connect(const char* host,uint32_t port);

	//收发消息
	uint32_t send(const void* buf,uint32_t len,uint32_t flags=0);
	uint32_t receive(void* buf,uint32_t len,uint32_t flags=0);

	uint32_t available() const;


	//套接字开始连接
	SOCKET accept(struct socketaddr* addr,uint32_t *addrlen);
	bool bind(uint32_t port);
	bool listen();

public:
	bool setLinger(uint32_t lingertime);
	bool setReuseAddr(bool on = true);
	bool getSocketError() const;
	bool setNonBlocking(bool on = true);                 //非阻塞（异步套接字）
public:
	SOCKET m_SocketID;                 
	SOCKADDR_IN m_SocketAddr;
	char m_Host[IP_SIZE];
	uint32_t m_Port;
}

//服务器socket
class ServerSocket
{
public:
	explicit ServerSocket(uint32_t port);
	~ServerSocket();
	
public:
	bool accept(Socket* socket);
	void setNonBlocking(bool on = true){m_Socket->setNonBlocking(on); }
	SOCKET getSOCKET() const {return m_Socket->getSOCKET();}
protected:
	Socket* m_Socket;
};

class SocketInputStream
{
public:
	SocketInputStream(Socket* socket,uint32_t InitBufferLen,uint32_t MaxBufferLen);
	virtual ~SocketInputStream();
public:
	uint32_t Read(char* buf,uint32_t len);
	bool ReadPacket(Packet& rPacket,uint32_t nSize,bool bIsCryptoPacket);

	bool Peek(char* buf,uint32_t len);

	bool Skip(uint32_t len);

	uint32_t Fill();

	void Initsize();
	bool Resize(int32_t &size);

	uint32_t Length() const;
private:
	Socket* m_pSocket;
	char* m_Buffer;

	uint32_t m_BufferLen;
	uint32_t m_InitBufferLen;
	uint32_t m_MaxBufferLen;

	uint32_t m_Head;
	uint32_t m_Tail;
}


class SocketOutputStream
{
public:
	SocketOutputStream(Socket* socket,uint32_t InitBufferLen,uint32_t MaxBufferLen);
	virtual ~SocketOutputStream();

public:
	uint32_t Write(const char* buf,uint32_t len);
	bool WritePacket(const Packet& rPacket,bool bIsCryptoPacket);
	bool WritePackerBuf(const PacketBuf& rPacket);

	uint32_t Flush();
	bool Resize(int32_t size);

	uint32_t Length() const ;

protected:
	Socket* m_pSocket;
	char* m_Buffer;

	uint32_t m_BufferLen;
	uint32_t m_InitBufferLen;
	uint32_t m_MaxBufferLen;

	uint32_t m_Head;
	uint32_t m_Tail;
}