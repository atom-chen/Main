#include <winsock.h>

int port = 9999;
void main()
{
	int sock = socket(PF_INET, SOCK_STREAM,IPPROTO_TCP);

	if (sock != -1)
	{
		listen(sock, 9999);
		while (1)
		{
			int length = 100;
			sockaddr buf[100];
			int new_sock = accept(sock, buf, &length);
			
		}
	}

	
}