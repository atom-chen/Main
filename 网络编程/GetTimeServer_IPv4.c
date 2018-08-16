#include <time.h>
typedef sockaddr SA

int main(int argc,char** argv)
{
	int listenFd,connFd;
	struct sockeraddr_in serverAddr;
	char buffer[1024];
	time_t ticks;

	listenFd = socket(AF_INET,SOCK_STREAM,0);
	bzero(&serverAddr,sizeof(serverAddr));
	serverAddr.sin_family = AF_INET;
	serverAddr.sin_addr.s_addr = htonl(INADDR_ANY);            //如果有多个IP，则在任意IP地址上接受连接
	serverAddr.sin_port = htons(13);                       

	bind(listenFd,(SA*)&serverAddr,sizeof(serverAddr));          //socket与文件描述符绑定
	listen(listenFd,LISTENQ);                                   //开始监听，LISTENQ表示内核最大允许排队的用户数量

	while(1)
	{
		connFd=accept(listenFd,(SA*)NULL,NULL);
		ticks = time(NULL);
		snprintf(buffer,sizeof(buffer),"%.24s\r\n",ctime(&ticks));        //格式转换
		write(connFd,buffer,strlen(buffer));
		close(listenFd);
	}
}