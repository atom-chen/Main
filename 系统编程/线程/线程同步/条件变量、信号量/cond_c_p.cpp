#include <stdio.h>
#include <unistd.h>
#include <stdlib.h>
#include <pthread.h>
#include <iostream>
#include <queue>
#include <forward_list>
using namespace std;

#define CONSUMER_COUNT 5

typedef struct 
{
	int sender;
	int msgId;
}Message;

pthread_cond_t cond = PTHREAD_COND_INITIALIZER;
pthread_mutex_t mutex = PTHREAD_MUTEX_INITIALIZER;
queue<Message> cache;
//生产者：产生数据
void* ProducerMain(void* para)
{
	forward_list<Message> buffer;
	buffer.clear();
	while(1)
	{
		int count = rand() % 10;
		for(int i=0;i<count;i++)
		{
			Message msg;
			msg.sender = rand() % 999999;
			msg.msgId = rand() % 1000;
			buffer.push_front(msg);
		}
		//加锁 往公共区域放入请求
		pthread_mutex_lock(&mutex);
		foreach(auto it = buffer.begin();it!=buffer.end();it++)
		{
			cache.push(*it);
		}
		pthread_mutex_unlock(&mutex);
		pthread_cond_signal(&cond);                  //通知消费者线程处理请求        
		buffer.clear();
		sleep(rand() % 2);		
	}
	return NULL;
} 

//消费者：处理数据
void* ConsumerMain(void* para)
{
	while(1)
	{
		pthread_mutex_lock(&mutex);
		while(cache.empty())                     //如果为空，就wait
		{
			pthread_cond_wait(&cond);
		}
		//不为空 并且wait成功 才会执行到这里
		Message msg = cache.front();
		cache.pop();
		pthread_mutex_unlock(&mutex);	
		
		printf("receive a package,sender = %u msgid = %U\n",msg.sender,msg.msgid);
		sleep(rand() % 8);
	}
	return NULL;
} 

int main(int argc, char const *argv[])
{
	pthread_t pid;
	pthread_t cid[CONSUMER_COUNT];
	pthread_create(&pid,NULL,ProducerMain,NULL);
	for(int i = 0;i<CONSUMER_COUNT;i++)
	{
	    pthread_create(cid+i,NULL,ConsumerMain,NULL);		
	}

	//join
	pthread_join(pid);
	for(int i = 0;i<CONSUMER_COUNT;i++)
	{
	    pthread_join(cid[i]);		
	}	
	return 0;
}