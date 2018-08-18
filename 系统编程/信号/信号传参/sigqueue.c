#include <stdio.h>
#include <signal.h>
#include <stdlib.h>

/*
typedef struct siginfo_t{ 
int si_signo;           //信号编号 
int si_errno;          //如果为非零值则错误代码与之关联 
int si_code;           //说明进程如何接收信号以及从何处收到 
pid_t si_pid;          //适用于SIGCHLD，代表被终止进程的PID 
pid_t si_uid;          //适用于SIGCHLD,代表被终止进程所拥有进程的UID 
int si_status;        //适用于SIGCHLD，代表被终止进程的状态 
clock_t si_utime;    //适用于SIGCHLD，代表被终止进程所消耗的用户时间 
clock_t si_stime;    //适用于SIGCHLD，代表被终止进程所消耗系统的时间 
sigval_t si_value; 
int si_int; 
void * si_ptr; 
void*  si_addr; 
int si_band; 
int si_fd; 
};
*/

void CatchInfo(int signal, siginfo_t *info, void *data)
{
	if(info!=NULL)
	{
		printf("si_value=%s\n",saval_t);
	}
	printf("%s\n",(char*)data);
}

int main(int argc, char const *argv[])
{
	pid_t pid=getpid();

	//捕捉信号
	sigaction act;
	act.sa_sigaction=CatchInfo;
	act.sa_flags=SA_SIGINFO;
	sigfillset(&(act.sa_mask));
	sigaction(SIG_USR1,&act,NULL);

	//给自己发送信号
	sigval para=9;
	sigqueue(pid,SIG_USR1,para);

	char buf[12];
	strcpy(buf,"aaaaaaa");
	sigval pPara=buf;
	sigqueue(pid,SIG_USR1,pPara);	


	pid=fork();
	if(pid>0)
	{
		//给子进程发个信号
	    sigqueue(pid,SIG_USR1,para);

	    sigval=buf;
	    sigqueue(pid,SIG_USR1,pPara);		
	}
	while(1)
	{
		sleep(1);
	}                     
	return 0;
}