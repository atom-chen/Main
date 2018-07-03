#include <stdio.h>
#include <string.h>
#include <unistd.h>
#include <sys/mman.h>
#include <stdlib.h>
#include <fcntl.h>

#define MMAPFILE "/tmp/data.1"

struct User
{
	int id;
	char name[32];
};
int main()
{
	struct User* add;
	struct User user;
	int fd;
	fd=open(MMAPFILE,O_RDWR);
	if(fd<0)
	{
		perror("open error");
		exit(-1);
	}
	add=mmap(NULL,sizeof(user),PROT_READ,MAP_SHARED,fd,0);// creatt 4b mapp
	if(add==MAP_FAILED)
	{
		perror("mmap error");
		exit(-3);
	}
	while(1)
	{
		memcpy(&user,add,sizeof(user));
		printf("userID=%u,username=%s\n",user.id,user.name);
		sleep(2);
	}
	return 0;
}