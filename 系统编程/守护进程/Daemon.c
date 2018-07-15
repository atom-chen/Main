#include <stdio.h>
#include <unistd.h>
#include <stdlib.h>
#include <string.h>
#include <fcntl.h>
void myDaemon()
{
	// 1
	pid_t pid=fork();
	if(pid>0)
	{
		exit(0);
	}

	// 2
	setsid();

	// 3
	int ret=chdir("/home/uzi/");
	if(ret==-1)
	{
		perror("chdir error!");
		exit(-3);
	}

	//4
	umask(0002);

	//5
	close(STDIN_FILENO);
	ret=open("/tmp/null2",O_RDWR | O_CREAT);
	if(ret<0)
	{
		perror("open file error!");
		exit(-5);
	}
	dup2(ret,STDOUT_FILENO);
	dup2(ret,STDERR_FILENO);
}


int main(int argc, char const *argv[])
{
	myDaemon();
	while(1)
	{
		sleep(5);
	}
	return 0;
}