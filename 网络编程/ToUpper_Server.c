#include<arpa/inet.h>
#include <sys/types.h>
#include <sys/socket.h>
#include <ctype.h>
#include <stdlib.h>

#define IP_TYPE AF_INET
#define PORT htons(6666)
#define ADDR htonl("127.0.0.1")
/*
    转大写 服务器
*/
int main(int argc, char const *argv[])
{
	int sSocket = socket(IP_TYPE,SOCK_STREAM,0);
	if(sSocket < 0)
	{
		printf("%s\n","socket error!");
		return -1;
	}
	struct sockaddr_in addr;
	addr.sin_family = IP_TYPE;
	addr.sin_port = PORT;
	addr.sin_addr.s_addr = ADDR;
	if(bind(sSocket,(sockaddr *)&addr,sizeof(addr)) < 0)
	{
		close(sSocket);
		printf("%s\n","bind error!");
		return -1;		
	}

	listen(sSocket,128);
	struct sockaddr_in cAddr;
	socklen_t addrlen = sizeof(cAddr); 
	//只接受一条连接
	int cSocket = accept(sSocket,(sockaddr *)&cAddr,&addrlen);
	if(cSocket>0)
	{
		char buffer[BUFSIZ];
		int len;
		while(1)
		{
			memset(buffer,0,sizeof(buffer));
			len = read(cSocket,buffer,sizeof(buffer));
			//处理数据
			for(int i = 0;i< len;i++)
			{
				buffer[i] = toupper(buffer[i]);
			}
			//回写
			write(cSocket,buffer,len);
		}
	}
	else
	{
		close(sSocket);
		printf("%s\n","accept error!");
		return -1;			
	}
	close(cSocket);
	close(sSocket);	
	return 0;
}

