
#include <iostream>
using namespace std;


//��ͳ�Ĵ��������
//throw int�����쳣
void my_strcpy1(char *to, char *from)
{
	if (from == NULL)
	{
		throw 1;
	}
	if (to == NULL)
	{
		throw 2;
	}

	//copy�ǵ� �������
	if (*from == 'a')
	{
		throw 3; //copyʱ����
	}
	while (*from != '\0')
	{
		*to = *from;
		to ++;
		from ++;
	}
	*to = '\0';
}

//��ͳ�Ĵ��������
//throw char*�����쳣
void my_strcpy2(char *to, char *from)
{
	if (from == NULL)
	{
		throw "Դbuf����";
	}
	if (to == NULL)
	{
		throw "Ŀ��buf����";
	}

	//copy�ǵ� �������
	if (*from == 'a')
	{
		throw "copy���̳���"; //copyʱ����
	}
	while (*from != '\0')
	{
		*to = *from;
		to ++;
		from ++;
	}
	*to = '\0';
}


class BadSrcType {};
class BadDestType {};
class BadProcessType
{
public:
	BadProcessType()
	{
		cout << "BadProcessType���캯��do \n";
	}


	BadProcessType(const BadProcessType &obj)
	{
		cout << "BadProcessType copy���캯��do \n";
	}

	~BadProcessType()
	{
		cout << "BadProcessType��������do \n";
	}

};


//��ͳ�Ĵ��������
//throw ����� �����쳣
void my_strcpy3(char *to, char *from)
{
	if (from == NULL)
	{
		throw BadSrcType();
	}
	if (to == NULL)
	{
		throw BadDestType();
	}

	//copy�ǵ� �������
	if (*from == 'a')
	{
		printf("��ʼ BadProcessType�����쳣 \n");
		throw BadProcessType(); //�᲻�����һ����������?
	}

	if (*from == 'b')
	{
		throw &(BadProcessType()); //�᲻�����һ����������?
	}

	if (*from == 'c')
	{
		throw new BadProcessType; //�᲻�����һ����������?
	}
	while (*from != '\0')
	{
		*to = *from;
		to ++;
		from ++;
	}
	*to = '\0';
}

void main()
{
	int ret = 0;
	char buf1[] = "cbbcdefg";
	char buf2[1024] = {0};

	try
	{
		//my_strcpy1(buf2, buf1);
		//my_strcpy2(buf2, buf1);
		my_strcpy3(buf2, buf1);
	}
	catch (int e) //e����д Ҳ���Բ�д
	{
		cout << e << " int�����쳣" << endl;
	}
	catch(char *e)
	{
		cout << e << " char* �����쳣" << endl;
	}

	//---
	catch(BadSrcType e)
	{
		cout << " BadSrcType �����쳣" << endl;
	}
	catch(BadDestType e)
	{
		cout << " BadDestType �����쳣" << endl;
	}
	//����1: ��� �����쳣��ʱ�� ʹ��һ���쳣����,��copy�����쳣����.  
	/*
	catch( BadProcessType e) //�ǰ���������copy��e ����e�����Ǹ���������
	{
		cout << " BadProcessType �����쳣" << endl;
	}
	*/
	//����2: ʹ�����õĻ� ��ʹ��throwʱ����Ǹ�����
	//catch( BadProcessType &e) //�ǰ���������copy��e ����e�����Ǹ���������
	//{
	//	cout << " BadProcessType �����쳣" << endl;
	//}

	//����3: ָ����Ժ�����/Ԫ��д��һ�� ��������/Ԫ�ز���д��һ��
	catch( BadProcessType *e) //�ǰ���������copy��e ����e�����Ǹ���������
	{
		cout << " BadProcessType �����쳣" << endl;
		delete e;
	}
	
	//����4: �����ʱ, ʹ�����ñȽϺ��� 

	// --
	catch (...)
	{
		cout << "δ֪ �����쳣" << endl;
	}

	cout<<"hello..."<<endl;
	system("pause");
	return ;
}


//��ͳ�Ĵ��������
int my_strcpy(char *to, char *from)
{
	if (from == NULL)
	{
		return 1;
	}
	if (to == NULL)
	{
		return 2;
	}
		
	//copy�ǵ� �������
	if (*from == 'a')
	{
		return 3; //copyʱ����
	}
	while (*from != '\0')
	{
		*to = *from;
		to ++;
		from ++;
	}
	*to = '\0';

	return 0;
}


void main41()
{
	int ret = 0;
	char buf1[] = "zbcdefg";
	char buf2[1024] = {0};

 	ret = my_strcpy(buf2, buf1);
	if (ret != 0)
	{
		switch(ret)
		{
		case 1:
			printf("Դbuf����!\n");
			break;
		case 2:
			printf("Ŀ��buf����!\n");
			break;
		case 3:
			printf("copy���̳���!\n");
			break;
		default:
			printf("δ֪����!\n");
			break;
		}
	}
	printf("buf2:%s \n", buf2);
	
	cout<<"hello..."<<endl;
	system("pause");
	return ;
}