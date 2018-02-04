#include<iostream>
#include <string>
using namespace std;
int* getNext(const string &zi);
int KMP(const string &fu, const string &zi, int pos=0)
{
	int i = pos;
	int j = 0;
	//��ȡNext����
	int *next = getNext(zi);
	//��ʼƥ��
	while (i < fu.length())
	{
		//�Ƚ�
		if (fu[i] == zi[j])
		{
			//����
			i++;
			j++;
			//�����ʱƥ������
			if (j == zi.length())
			{
				return i - j + 1;
			}
		}
		//����ʱʧ��->�˻�ǰһ������ƥ���м������
		else if(j>0){
			j = next[j - 1];
		}
		//���˻ؿ�ͷ�˻���ʧ��->iֱ�Ӻ���
		else{
			i++;
		}
	}
	return -1;
}
//�õ��Ӵ���next���顪����jλ��ʧ�䣬��ǰ���м����������
//ǰ����0�������Ϊ0����1�������Ϊ1...
int* getNext(const string &zi)
{
	int *next = new int[zi.length()];
	//ǰ׺����
	int prefix = 0;
	//��׺����
	int suffix = 1;
	next[0] = 1;
	while (suffix < zi.length())
	{
		//������
		if (zi[prefix] == zi[suffix])
		{
			next[suffix] = suffix + 1;
			//��� �������±Ƚ�
			prefix++;
			suffix++;
		}
		//����ȵ����
		else if (prefix > 0)
		{
			//��˷����ǰ�ַ���ǰ׺���׺������������ڴ˴�����P��T�Ĳ�ƥ�䣬���ҵ�next[prefix-1]��λ�ÿ�ʼ��һ��ƥ��
			prefix = next[prefix - 1];
		}
		else{
			//�����ʱ�ڸ��±�0�Ƚ��Ҳ����->next[i]=0
			next[prefix] = 0;
			suffix++;
		}
	}
	
		
	return next;
}
void main()
{
	string fu = "WangYiBaBa";
	string zi = "Yi";
	cout<<KMP(fu,zi)<<endl;
}