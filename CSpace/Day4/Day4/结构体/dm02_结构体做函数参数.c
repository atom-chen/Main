#define  _CRT_SECURE_NO_WARNINGS 
#include <stdlib.h>
#include <string.h>
#include <stdio.h>


typedef struct Teacher
{
	char name[64];
	int age ;
	int id;
}Teacher;

void printTeacher(Teacher *array, int num)
{
	int i = 0;

	for (i=0; i<num; i++)
	{
		printf("age:%d \n", array[i].age);
	}
}

void sortTeacer(Teacher *array, int num)
{
	int		i,j;
	Teacher tmp;

	for (i=0; i<num; i++)
	{
		for (j=i+1; j<num; j++)
		{
			if (array[i].age > array[j].age)
			{
				tmp = array[i];  //=�Ų���  ��ֵ����
				array[i] = array[j];
				array[j] = tmp;
			}
		}
	}
}


// �ṹ������ 3  ������ʦ�����䣬����
void main22()
{
	int			i = 0;
	Teacher		Array[3];  //��stack �����ڴ�
	int			num = 3;

	for (i=0; i<num; i++)
	{
		printf("\nplease enter age:");
		scanf("%d", &(Array[i].age) );
	}

	//��ӡ��ʦ����
// 	for (i=0; i<num; i++)
// 	{
// 		printf("age:%d \n", Array[i].age);
// 	}

	printTeacher(Array, num);

	sortTeacer(Array, num);

	printf("����֮��\n");

	printTeacher(Array, num);

	printf("hello...\n");
	system("pause");
	return ;
}

Teacher * createTeacher(int num)
{
	Teacher * tmp = NULL;
	tmp = (Teacher *)malloc(sizeof(Teacher) * num); //	Teacher		Array[3]
	if (tmp == NULL)
	{
		return NULL;
	}
	return tmp; //

}

void FreeTeacher(Teacher *p)
{
	if (p != NULL)
	{
		free(p);
	}
}

void main233()
{
	int			i = 0;
	//Teacher		Array[3];  //��stack �����ڴ�
	int			num = 3;
	Teacher *pArray = NULL;
	pArray = createTeacher(num);

	for (i=0; i<num; i++)
	{
		printf("\nplease enter age:");
		scanf("%d", & (pArray[i].age) );
	}

	//��ӡ��ʦ����
	// 	for (i=0; i<num; i++)
	// 	{
	// 		printf("age:%d \n", Array[i].age);
	// 	}

	printTeacher(pArray, num);

	sortTeacer(pArray, num);

	printf("����֮��\n");

	printTeacher(pArray, num);

	FreeTeacher(pArray);

	printf("hello...\n");
	system("pause");
	return ;
}