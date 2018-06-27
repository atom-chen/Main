#include <unistd.h>
#include <stdio.h>
#include <stdlib.h>
#include <sys/time.h>
#include <signal.h>

sigset_t m_set;
sigset_t unjueset;
void Catch(int signalID)
{
	printf("id=%u\n",signalID);
}

void printPending(sigset_t *set)
{
	for(int i=1;i<32;i++)
	{
		printf("%u",sigismember(set,i));
	}
	printf("\n");
}

int main(int argc, char const *argv[])
{
	int i;
	signal(SIGALRM,Catch);
	if(sigaddset(&m_set,SIGQUIT)==0 && sigaddset(&m_set,SIGALRM)==0 && sigaddset(&m_set,SIGTSTP)==0)
	{
		sigset_t oldset;
		if(sigprocmask(SIG_BLOCK,&m_set,&oldset)==0)
		{
			printf("signal is mask\n");
		}
		else
		{
			printf("signal mask error\n");
			exit(-1);
		}
	}
	else
	{
		printf("signal mask error\n");
		exit(-1);
	}

	sigemptyset(&m_set);
	struct itimerval newVal,oldVal;
	newVal.it_value.tv_sec=1.5f;
	newVal.it_value.tv_usec=0;
	newVal.it_interval.tv_sec=1;
	newVal.it_interval.tv_usec=0;
	setitimer(ITIMER_REAL,&newVal,&oldVal);
	while(1)
	{
		if(sigpending(&unjueset)==0)
		{
			//print wei jue xin hao ji
			printPending(&unjueset);
		}
		sleep(1);
	}
	return 0;
}