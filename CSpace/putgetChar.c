#include <stdio.h>
#include <stdlib.h>
#include <ctype.h>

int main()
{
	int c;
	while ((c = getchar() )!= EOF)
	{
		putchar(tolower(c));
		if(c=='q')
		{
			return 1;
		}
	}
	return 22;
}