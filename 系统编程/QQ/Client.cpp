/*
this is QQ Client
*/
#include <stdio.h>
#include <unistd.h>
#include <stdlib.h>
#include <sys/types.h>
#include <sys/stat.h>
#include <string.h>
#include <fcntl.h>

#define ONLINE 0
#define DOWMLINE 1
#define CHAT 2

#define SERVER_ADDR /home/uzi/桌面/gitSPace/系统编程/QQ/ServerFifo
#define CLIENT_ADDR_ROOT "/tmp/fifo.";
#define FILE_MODEL (S_IRUSR | S_IWUSR | S_IRGRP | S_IWOTH)

struct MSG
{
	int send;
	int receive;
	int msgCode;
	char data[1024];
}

void SendRequest(struct MSG msg);
void Response(struct MSG msg);
int main(int argc, char const *argv[])
{
	pid_t pid=getpid();
	char buffer[1024];
	stacpy(buffer,CLIENT_ADDR_ROOT);
	strcat(buffer,(char*)&pid);
	//receive
	if(mkfifo(buffer,FILE_MODEL)==-1)
	{
		perror("server mkpipe1 error\n");
		exit(-1);
	}

	int fd_write=open(SERVER_ADDR,O_WRONLY);
	int fd_read=open(buffer,O_RDWR);

	if(fd_write>0 && fd_read>0)
	{

		struct MSG msg;
		//send online package
		memset(&msg,0,sizeof(MSG));
		msg.send=pid;
		msg.receive=-1;
		strcpy(msg.data,buffer);
		SendRequest(msg,fd_write);
		int ret;
		int receive;
		while(1)
		{
			memset(&msg,0,sizeof(msg));
			//read input
			ret=read(STDIN_FILENO,buffer,4);
			if(ret>0)
			{
				receive=atoi(buffer);
				ret=read(STDIN_FILENO,buffer,1024);
				if(ret>0)
				{
					msg.send=pid;
					msg.receive=receive;
					strcpy(msg.data,);
					SendRequest(msg,fd_write);
					memset(&msg,0,sizeof(msg));
				}
			}
			//receive from server
			ret=read(fd_read,&msg,1024);;
			if(ret>0)
			{
				Response(msg);
			}
		}
	}
	return 0;
}
void SendRequest(struct MSG msg,int fd_write)
{
	write(fd_write,msg,sizeof(msg));
}
void Response(struct MSG msg)
{
	switch(msg.msgCode)
	{
		case CHAT:
		printf("%u said:%s\n",msg.send,msg.data);
		break;
	}
}