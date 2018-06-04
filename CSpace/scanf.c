#include <stdio.h>

int main()
{
	int sum,val;
	sum=0;
	while(scanf("%d",&val)==1)
	{
		printf("%.2d\n",sum+=val);
	}
	return 0;
}