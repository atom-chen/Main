#include <unistd.h>
#include <stdio.h>
#include <stdlib.h>
#include <sys/time.h>

int main(int argc, char const *argv[])
{
	int i;
	struct itimerval newVal,oldVal;
	newVal.it_value.tv_sec=1;
	newVal.it_value.tv_usec=0;
	newVal.it_interval.tv_sec=0;
	newVal.it_interval.tv_usec=0;
	setitimer(ITIMER_REAL,&newVal,&oldVal);
	for(i=1;;i++)
	{
		printf("%d\n",i);
	}
	return 0;
}