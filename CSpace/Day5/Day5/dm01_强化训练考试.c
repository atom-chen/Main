

#define  _CRT_SECURE_NO_WARNINGS 
#include <stdlib.h>
#include <string.h>
#include <stdio.h>


int sort(char **myp1 /*in*/, int num1, char (*myp2)[30], int num2, char ***myp3, int *num3)
{
	int i = 0, j = 0, k= 0;
	int  tmplen = 0;
	char **p3 = NULL;
	char *tmpP = NULL;
	p3 = (char **)malloc( (num1 + num2) * sizeof(char *)  ); //����װ����ָ��
	if (p3 == NULL) 
	{
		return -1;
	}
	//����

	for (i=0; i<num1; i++)
	{
		tmplen= strlen(myp1[i]) + 1;
		p3[i] = (char *)malloc( tmplen * sizeof(char)) ;
		if (p3[i] == NULL)
		{
			return -2;
		}
		strcpy(p3[i], myp1[i]);
	}


	for (j=0; j<num2; j++, i++)
	{
		tmplen = strlen(myp2[j]) + 1;
		p3[i] = (char *)malloc (tmplen * sizeof(char));
		if (p3[i] == NULL)
		{
			return -3;
		}
		strcpy(p3[i], myp2[j]);
	}

	tmplen = num1 + num2;
	//*num3 = num1 + num2;

	//����
	for (i=0; i<tmplen; i++)
	{
		for (j=i+1; j<tmplen; j++)
		{
			if ( strcmp(p3[i], p3[j]) > 0 )
			{
				tmpP = p3[i];
				p3[i] = p3[j];
				p3[j] = tmpP;
			}
		}
	}

	//��Ӹ�ֵ
	*num3 = tmplen;
	*myp3 = p3;
	return 0;
}


void sortFree01(char **p, int len)
{
	int i = 0;
	if (p == NULL)
	{
		return ;
	}

	for (i=0; i<len ; i++)
	{
		free(p[i]);
	}
	free(p);
}

void sortFree02(char ***myp, int len) //�Ѷ���ָ��ָ���ά�ڴ��ͷŵ�,,ͬʱ��ӵ��޸���ʵ�ε�ֵ
{
	int i = 0;
	char **p = NULL;
	if (myp == NULL)
	{
		return ;
	}

	p  = *myp; //��ԭ�ɶ���ָ��
	if (p == NULL)
	{
		return ;
	}

	for (i=0; i<len ; i++)
	{
		free(p[i]);
	}
	free(p);
	//myp ��ʵ�εĵ�ַ
	*myp = NULL; //��Ӹ�ֵ��ָ����ڵ��������
}

int  main()
{
	int ret = 0;
	char *p1[] = {"aa", "ccccccc", "bbbbbb"};
	char buf2[10][30] = {"111111", "3333333", "222222"};
	char **p3 = NULL;
	int len1, len2, len3, i = 0;

	len1 = sizeof(p1)/sizeof(*p1);
	len2 = 3;

	ret = sort(p1, len1, buf2, len2, &p3, &len3);
	if (ret != 0)
	{
		printf("func sort() err:%d \n", ret);
		return ret;
	}

	for (i=0; i<len3; i++)
	{
		printf("%s\n", p3[i]);
	}

	printf("hello...\n");
	system("pause");
	return ret;
}