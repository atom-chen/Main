#include <unistd.h>
#include <stdio.h>
#include <stdlib.h>

int main(int argc, char const *argv[])
{
	int i;
	alarm(1);
	for(i=1;;i++)
	{
		printf("%d\n",i);
	}
	return 0;
}