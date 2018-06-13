/*
 * Environ.c
 *
 *  Created on: 2018年5月25日
 *      Author: uzi
 */
#include <stdio.h>
#include<unistd.h>
extern char** environ;

int main()
{
	int i;
	for(i=0;environ[i]!=NULL;i++)
	{
		printf("%s\n",environ[i]);
	}
	return 0;
}
