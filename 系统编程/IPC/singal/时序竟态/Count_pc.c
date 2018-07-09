#include <stdio.h>
#include <stdlib.h>
#include <signal.h>

pid_t pid;
int data=0;
void ParentCatch(int signal)
{
	kill(pid,SIGUSR1);
	printf("%s %u\n","parent said data=",data);
	data+=2;
}
void ChildCatch(int signal)
{
	data+=2;
	printf("%s %u\n","child said data=",data);
	kill(pid,SIGUSR1);
}

int main(int argc, char const *argv[])
{

	pid=fork();
	if(pid>0)
	{
		struct sigaction act,old;
		act.sa_handler=ParentCatch;
		sigemptyset(&(act.sa_mask));
		act.sa_flags=0;
		sigaction(SIGUSR1,&act,&old);

		data=1;
		ParentCatch();
		while(1)
		{
			sleep(5);
		}		
	}
	else if(pid==0)
	{
		struct sigaction act,old;
		act.sa_handler=ChildCatch;
		sigemptyset(&(act.sa_mask));
		act.sa_flags=0;
		sigaction(SIGUSR1,&act,&old);	
        pid=getppid();
		while(1)
		{
			sleep(5);
		}		
	}
	return 0;
}