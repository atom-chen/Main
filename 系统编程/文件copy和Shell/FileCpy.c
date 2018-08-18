#include <stdlib.h>
#include <stdio.h>
#include <fcntl.h>

#define SRCFILE "/home/uzi/桌面/gitSPace/OpenGL/论文.docx"
#define TARGETFILE "/home/uzi/桌面/gitSPace/系统编程/cpy.docx"

int main(int argc, char const *argv[])
{
	char buffer[1024];//1 kb
	int fd_r,fd_w;
	int ret;
	fd_r=open(SRCFILE,O_RDONLY);
	fd_w=open(TARGETFILE,O_WRONLY);
	if(fd_r || fd_w)
	{
	   	while((ret=read(fd_r,buffer,1024))>0)
	    {
		    write(fd_w,buffer,ret);
	    }	
	}
	else
	{
		printf("%s\n","write error");
	}
	return 0;
}