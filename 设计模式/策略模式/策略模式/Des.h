#include "Encode.h"

class Des :public Encode
{
public:
	virtual void Code(const char* const password)
	{
		printf("DES�����㷨:%s\n", password);
	}
protected:
};