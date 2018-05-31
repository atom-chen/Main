#include <stdio.h>
#include <unistd.h>
#include<stdlib.h>
#include <sys/types.h>
#include <sys/stat.h>
#include <string.h>
#include <fcntl.h>

#define SHAREFILE "/tmp/share.1"

int main()
{
	pid_t pid;
	int fd;
	fd=open(SHAREFILE,O_RDWR);
	if(fd==-1)
	{
		perror("open file error\n");
		exit(-1);
	}
	pid=fork();
	//child
	if(pid==0)
	{
		int ret;
		char buffer[1024];
		while(1)
		{
			ret=read(fd,buffer,1024);
			if(ret>0)
			{
			    printf("ret=%d,text=%.*s\n",ret,ret,buffer);
			}
			sleep(5);
			//write(STDOUT_FILENO,buffer,ret);
		}
		printf("ret=%d,text=%.*s\n",ret,ret,buffer);
	}
	else
	{
		int ret;
		char buffer[1024];
		while((ret=read(STDIN_FILENO,buffer,1024))>0)
		{
			write(fd,buffer,ret);
		}
	}
	return 0;
}