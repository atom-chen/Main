/*
this is QQ Server
*/
#include <stdio.h>
#include <unistd.h>
#include <stdlib.h>
#include <sys/types.h>
#include <sys/stat.h>
#include <string.h>
#include <fcntl.h>
#include <map>

#define ONLINE 0
#define DOWMLINE 1
#define CHAT 2


#define SERVER_ADDR /home/uzi/桌面/gitSPace/系统编程/QQ/ServerFifo
#define FILE_MODEL (S_IRUSR | S_IWUSR | S_IRGRP | S_IWOTH)
struct MSG
{
	int send;
	int receive;
	int msgCode;
	char data[1024];
}
void Server(struct MSG msg);
void SendRes(struct MSG msg);

struct  Client
{
	int fd;
};

std::map<int,Client> clientAddr;  //key=pid value=addr

int main(int argc, char const *argv[])
{
	if(mkfifo(SERVER_ADDR,FILE_MODEL)==-1)
	{
		perror("server mkpipe1 error\n");
		exit(-1);
	}
	int fd=open(SERVER_ADDR,O_RDWR);
	if(fd>0)
	{
		int ret;
        struct MSG msg;
        memset(&msg,0,sizeof(MSG));
		while(1)
		{
			ret=read(fd,&msg,sizeof(MSG));//轮询
			//if has msg
			if(ret!=0)
			{
				Server(msg);
			}
            memset(msg,0,sizeof(MSG));
		}
	}
	return 0;
}
void Server(struct MSG msg)
{
	switch(msg.msgCode)
	{
	case ONLINE:
	    struct Client client;
	    //try connect to client fifo
	    int fd=open(msg.data,O_WRONLY);
	    if(fd>0)
	    {
	        client.fd=fd;
	        clientAddr.insert(std::pair<int,Client>(msg.send,client));
	    }
		break;
	case DOWMLINE:
        auto it=clientAddr.find(msg.send);
        if(it!=clientAddr.end())
        {
        	close(it->fd);
        	clientAddr.erase(it);
        }
		break;
	case CHAT:
        SendRes(msg);
		break;
	}
}
void SendRes(struct MSG msg)
{
	auto it=clientAddr.find(msg.send);
	if(it!=clientAddr.end())
	{
		write(it->fd,&msg,sizeof(msg));
	}
}