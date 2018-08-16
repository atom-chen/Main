

#define MAXLINE 1023
#define TIME_PORT 13
typedef sockaddr SA

int main(int argc, char const *argv[])
{
	int socketFd,n;
	char buffer[MAXLINE + 1];
	struct socketaddr_in serveraddr;
	if(argc < 2)
	{
		exit(-1);
	}
	if((socketFd = socket(AF_INET,SOCK_STREAM,0))<0)
	{
		perror("socket eof!");
		exit(-1);
	}
	bzero(&serveraddr,sizeof(serveraddr));
	serveraddr.sin_family = AF_INET;                  //协议簇，AF_INET表示TCP/IP协议
	serveraddr.sin_port = htons(TIME_PORT);              //二进制格式写入端口号

	//二进制格式写入Ip地址
	if(inet_pton(AF_INET , argv[1],&serveraddr.sin_addr) <= 0)
	{
		perror("inet_pton eof!");
		exit(-2);
	}

	//连接成功
	if(connect(socketFd,(SA*)&serveraddr,sizeof(serveraddr)) < 0)           //socket与文件描述符绑定
	{
		perror("connect eof!");
		exit(-3);
	}
	

	while((n = read(socketFd,buffer,MAXLINE)) > 0)
	{
		buffer[n] ='\0';
		if(fputs(buffer,STDOUT_FILENO) == EOF)
		{
			perror("socket close!");
			exit(-4);
		}
	}
	if(n<0)
	{
		perror("read error!");
		exit(-5);
	}
	return 0;
}