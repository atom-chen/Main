#include <stdio.h>
#include <stdlib.h>
#include <unistd.h>
#include <signal.h>

void CatchInfo(int signal, siginfo_t *info, void *data)
{
	printf("%s\n","信号捕捉函数被调用\n");
}

int main(int argc, char const *argv[])
{
	//先捕捉信号
	sigaction act;
	act.sa_sigaction=CatchInfo;
	act.sa_flag=SA_SIGINFO | SIG_RESTART;
	sigfillset(&(act.sa_mask));
	sigaction(SIGINT,&act,NULL);

	int ret;
	char buffer[1024];
	while(ret=read(STDIN_FILENO,buffer,1024))
	{

		printf("console said:%s\n",buffer);
	}
	if(ret==-1)
	{
	    printf("%s\n","read函数被信号打断");	
	}
	return 0;
}