#include <stdio.h>
#include <unistd.h>
#include <stdlib.h>
#include <pthread.h>
#include <string.h>

typedef struct 
{
	int val;
	char buf[128];
}exit_t;

//通过return自行退出
void* routineMain_return(void* data)
{
	if(data!=NULL)
	{
		exit_t* retval =(exit_t*)data;
		retval.val=9;
	    strcpy(retval.buf,"Un Safe Ret!");
	    return 0;			
	}
	return -1;
}

//通过pthread_exit自行退出
void* routineMain_exit(void* data)
{
	if(data!=NULL)
	{
		exit_t *retval = (exit_t*)data;
		retval->val=9;
		strcpy(retval->buf,"Safe Ret!");	
		pthread_exit((void*)0);	
	}
	pthread_exit(-1);
}

//通过pthread_exit自行退出
void* routineMain_cancel(void* data)
{
	while (1) 
	{
		//printf("thread 3: I'm going to die in 3 seconds ...\n");
		//sleep(1);
		pthread_testcancel();	//自己添加取消点*/
	}
}

int main(int argc, char const *argv[])
{
	pthread_t thread;
	int ret;
	exit_t *pRet=(exit_t *)malloc(sizeof(exit_t));

	//线程1
	pthread_create(&thread,NULL,routineMain_return,(void*)pRet);		
	pthread_join(thread,(void**)&ret);
	printf("one : val = %u,buf= %s ,ret= %u \n",pRet->val,pRet->buf,ret);


    //线程2
	pthread_create(&thread,NULL,routineMain_exit,(void*)pRet);		
	pthread_join(thread,(void**)&ret);
	printf("two : val = %u,buf= %s ,ret= %u \n",pRet->val,pRet->buf,ret);



	//线程3
	pthread_create(&thread, NULL, routineMain_cancel, NULL);
    pthread_cancel(thread);                   //发送取消该线程的请求，会在线程主控函数遇到检查点时，终止该线程
	pthread_join(thread, &ret);
	printf("three : val = %u,buf= %s ,ret= %u \n",pRet->val,pRet->buf,ret);



	if(pRet != NULL)
	{
	    free(pRet);
	    pRet=NULL;		
	}	
}