#include<stdio.h>
#include<stdlib.h>
#include<string.h>
#include<unistd.h>
#include<signal.h>

void SigMask2PCB(int signal,int isMask=1,sigset_t *old=NULL)       //设置PCB信号屏蔽字的工具函数
{
	if(isMask)
	{
		sigset_t set;
		sigemptyset(&set);
		sigaddset(&set,signal);
		sigprocmask(SIGBLOCK,&set,old);
	}
	else
	{
		sigset_t set;
		sigfillset(&set);
		sigdelset(&set,signal);
		sigprocmask(UNBLOCK,&set,old);
	}
}

typedef void(*SIGCATCH)(int);

void CatSignal(int signal,SIGCATCH func)         //注册捕捉的工具函数
{
	struct sigaction act,old;
	int ret;
	act.sa_handler=func;
	sigfillset(&(act.sa_mask));
	act.sa_flags=0;
}

void Catch(int signal)
{
	int status;pid_t pid;
	if((pid=(waitpid(0,&status,WNOHANG)))>0)                     //只有一个sigchld 所以可以用if
	{
		if (WIFEXITED(status))
		{
			printf("child %d exit %d\n", pid, WEXITSTATUS(status));
		}
		else if (WIFSIGNALED(status))
		{
			printf("child %d cancel signal %d\n", pid, WTERMSIG(status));
		}
	}
}

int main()
{
	int fd[2];
	pid_t pid;
	int ret=pipe(fd);
	if(ret == -1)
	{
		perror("pipe error\n");
		exit(22);
	}

	SigMask2PCB(SIGCHLD,1);
	pid = fork();

	//接收输入，后结束
	if(pid > 0)
	{		
		close(fd[1]);
		char buffer[1024];
		dup2(fd[0],STDIN_FILENO);                            //input重定向：从pipe中读取数据
		CatSignal(SIGCHLD,Catch);
		SigMask2PCB(SIGCHLD,0);
		execlp("wc","wc","-l",NULL);                        //command read from fd[0]
	}
	//触发
	else
	{
		sleep(1);
		close(fd[0]);
	    dup2(fd[1],STDOUT_FILENO);                           //输出重定向
	    execlp("ls","ls",NULL);
	}
}
