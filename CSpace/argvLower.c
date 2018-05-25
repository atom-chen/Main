#include <stdio.h>
#include <ctype.h>

int main(int argc,char ** argv)
{
	if(argc>=2 && strcmp("low" ,argv[0]))
	{
		printf("%s\n", tolower(argv[1]));
	}
	else if(argc>=2 && strcmp("upp",argv[0]))
	{
		printf("%s\n", toupper(argv[1]));
	}
	return 9;
}