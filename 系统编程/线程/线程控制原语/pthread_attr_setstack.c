#include <stdio.h>
#include <unistd.h>
#include <stdlib.h>
#include <pthread.h>
#include <string.h>

#define THREAD_STACK_SIZE 0x100000

void *tfn(void *arg)
{
	int n = 3;

	while (n--) 
	{
		printf("thread count %d\n", n);
		sleep(1);
	}

    pthread_exit((void *)1);
}

int main(void)
{
	pthread_t tid;
	pthread_attr_t attr;
	void *tret;
	int err;

	//set
	pthread_attr_setdetachstate(&attr,PTHREAD_CREATE_DETACHED);
	char* stack = malloc(THREAD_STACK_SIZE);
	pthread_attr_setstack(&attr,stack,THREAD_STACK_SIZE);

	//create
	pthread_create(&tid, &attr, tfn, NULL);

	//get
	int size = 0 , detachState = 0;
	char* retStack = NULL;
	pthread_attr_getdetachstate(&attr,&detachState);
	pthread_attr_getstack(&attr,&retStack,&size);
	printf("%s %x %u\n","threaad stack addr =",retStack,size);
	pthead_attr_getstacksize(&attr,&size);
	printf("%s %u\n","size = ",size);
 
	if(detachState == PTHREAD_CREATE_DETACHED)
	{
		printf("%s\n","线程分离!");
	}
	else
	{
		printf("%s\n","线程未分离!");
	}
	sleep(5);
	pthread_attr_destroy(&attr);
	if(stack!=NULL)
	{
		free(stack);
		stack = NULL;
	}
	return 0;
}
