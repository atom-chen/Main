#include <stdio.h>
#include <stdlib.h>

char* getstr1()
{
	char *p1 = "abc";
	return p1;
}
char* getstr2()
{
	char *p2 = "abc";
	return p2;
}
int main()
{
	char *p1 = NULL;
	char *p2=NULL;
	p1 = getstr1();
	p2 = getstr2();
	printf("p1=%s,&p1=%s,p2=%s,&p2=%s", *p1, p1, *p2, p2);

	return 0;
}