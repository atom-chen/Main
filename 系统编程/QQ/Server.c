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


#define SERVER_ADDR /home/uzi/桌面/gitSPace/系统编程/QQ/ServerFifo
#define FILE_MODEL (S_IRUSR | S_IWUSR | S_IRGRP | S_IWOTH)
struct MSG
{
	int send;
	int receive;//id=-1:to server
	char data[1024];
}
void Server(struct MSG msg);


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

}
