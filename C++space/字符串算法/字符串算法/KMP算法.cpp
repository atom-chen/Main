#include<iostream>
#include <string>
using namespace std;
int* getNext(const string &zi);
int KMP(const string &fu, const string &zi, int pos=0)
{
	int i = pos;
	int j = 0;
	//获取Next数组
	int *next = getNext(zi);
	//开始匹配
	while (i < fu.length())
	{
		//比较
		if (fu[i] == zi[j])
		{
			//后移
			i++;
			j++;
			//如果此时匹配完了
			if (j == zi.length())
			{
				return i - j + 1;
			}
		}
		//若此时失配->退回前一个数的匹配有几个相等
		else if(j>0){
			j = next[j - 1];
		}
		//若退回开头了还在失配->i直接后移
		else{
			i++;
		}
	}
	return -1;
}
//拿到子串的next数组——在j位置失配，则看前面有几个和它相等
//前面有0个相等则为0，有1个相等则为1...
int* getNext(const string &zi)
{
	int *next = new int[zi.length()];
	//前缀索引
	int prefix = 0;
	//后缀索引
	int suffix = 1;
	next[0] = 1;
	while (suffix < zi.length())
	{
		//如果相等
		if (zi[prefix] == zi[suffix])
		{
			next[suffix] = suffix + 1;
			//相加 继续往下比较
			prefix++;
			suffix++;
		}
		//不相等的情况
		else if (prefix > 0)
		{
			//回朔：当前字符的前缀与后缀不连续，如果在此处发生P与T的不匹配，则找到next[prefix-1]的位置开始下一次匹配
			prefix = next[prefix - 1];
		}
		else{
			//如果此时在跟下标0比较且不相等->next[i]=0
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