#include <stdio.h>
#include <pthread.h>
#include <stdlib.h>
#include <unistd.h>
#include <sys/mman.h>
#include <fcntl.h>
#include <semaphore.h>

#define FILE_NAME mt_test
#define C_COUNT 5
#define P_COUNT 1
#define MAX 200

typedef struct 
{
	int data;
	sem_t sem;
}Sem;

Sem* pBox = NULL;
Sem* pItem = NULL;
void C()
{
	while(1)
	{
		sem_wait(&pItem->sem);
		pLock-> data -=rand() % 10;
		int data = pLock ->data;
		sem_post(&pBox->sem);	
		printf("data = %u\n",data);
		sleep(rand() % 8);
	}
}

void P()
{
	while(1)
	{
		sem_wait(&pBox->sem);
		pLock-> data +=rand() % 50;
		int data = pLock ->data;
		sem_post(&pItem->sem);	
		printf("data = %u\n",data);
		sleep(1);
	}
}

int main(int argc, char const *argv[])
{
	//mmap
	int fd = open(FILE_NAME,O_CREAT | O_RDWR, 0777);
	if(fd < 0)
	{
		perror("open file error!")
		exit(-1);
	}
	ftruncate(fd,sizeof(Mutex)*2);
	pBox = mmap(NULL,sizeof(Mutex)*2,PROT_READ|PROT_WRITE,MAP_SHARED,fd,0);
	if(pLock == MAP_FAILED || pLock == NULL)
	{
		perror("mmap error!")
		exit(-1);
	}
	close(fd);
	pItem = pBox + 1;
	memset(pBox,0,sizeof(Mutex)*2);

	//初始化mmap返回的内存地址属性
	sem_init(&pItem-> sem,1,0);
	sem_init(&pBox-> sem,1,MAX);

	for(int i=0;i<P_COUNT;i++)
	{
		pid_t pid = fork();
		if(pid == 0)
		{
			P();
			return;
		}		
	}

	for(int i=0;i<C_COUNT;i++)
	{
		pid_t pid = fork();
		if(pid == 0)
		{
			C();
			return;
		}		
	}
	while(1)
	{
		sleep(5);
	}
	munmap(pBox,sizeof(Mutex)*2);
	sem_destroy(&pBox-> sem);
	sem_destroy(&pItem-> sem);	
	return 0;
}