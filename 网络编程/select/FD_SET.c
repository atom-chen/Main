#include <stdio.h>
#include <sys/select.h>
#include <unistd.h>


int main(int argc, char const *argv[])
{
	fd_set fdset;
	FD_ZERO(&fdset);          //将set清零

	FD_SET(1,&fdset);         //将fd加入set集合
	FD_SET(2,&fdset);
	FD_SET(3,&fdset);
	FD_SET(7,&fdset);

	int isset=FD_ISSET(3,&fdset);
	if(isset==0)
	{
		printf("%u %s\n",3,"is not in fdset");
	}

	FD_CLR(3,&fdset);
	int isset=FD_ISSET(3,&fdset);
	if(isset==0)
	{
		printf("%u %s\n",3,"is not in fdset");
	}

	FD_ZERO(&fdset);
	return 0;
}