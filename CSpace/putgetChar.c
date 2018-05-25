#include <stdio.h>
#include <stdlib.h>
#include <ctype.h>

int main()
{
	int c;
	while ((c = getchar() )!= EOF)
	{
		putchar(tolower(c));
	}
	return 22;
}