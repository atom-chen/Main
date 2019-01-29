#include <unistd.h> #include <stdio.h> #include <stdlib.h> #include <signal.h>

void Catch(int signal)
{
	printf("catch signal %u\n",signal);
}

int mySleep(float second)
{
	struct sigaction act,old;
	int ret;
	act.sa_handler=Catch;
	sigemptyset(&(act.sa_mask));
	act.sa_flags=0;

	ret=sigaction(SIGALRM,&act,&old);
	alarm(second);
  //signal(SIGALRM,Catch);
	if(pause()==-1)
	{
		printf("%s\n","pause return value = -1");

		ret=sigaction(SIGALRM,&old,NULL);
	}
	ret=alarm(0);
	return ret;
}

int main(int argc, char const *argv[])
{
	while(1)
	{
	   mySleep(2);
	   printf("%s\n","my sleep call over");		
	}

	return 0;
}