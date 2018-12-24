#include<arpa/inet.h>
#include <sys/types.h>
#include <sys/socket.h>
#include <ctype.h>
#include <stdlib.h>

#define IP_TYPE AF_INET
#define PORT htons(6666)
#define ADDR "127.0.0.1"
/*
    转大写 客户端
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
	bzero(&addr,sizeof(addr));
	addr.sin_family = IP_TYPE;
	addr.sin_port = PORT;
	inet_pton(IP_TYPE,ADDR,&addr.sin_addr.s_addr);

	connect(sSocket,(struct sockaddr *)&addr,sizeof(addr));
	char buffer[BUFSIZ];
	int len;
	while(1)
	{
		bzero(buffer,sizeof(buffer));

		//读玩家输入
		len = read(STDIN_FILENO,buffer,sizeof(buffer));
		//写到流里
		write(sSocket,buffer,len);
		memset(buffer,0,sizeof(buffer));
		//读服务器回传
		len = read(sSocket,buffer,sizeof(buffer));
		//打印
		write(STDOUT_FILENO,buffer,len);
	}

	close(sSocket);
	return 0;
}

