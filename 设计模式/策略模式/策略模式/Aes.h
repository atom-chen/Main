#include "Encode.h"

class Aes :public Encode
{
public:
	virtual void Code(const char* const password)
	{
		printf("AES�����㷨:%s\n", password);
	}
protected:
};