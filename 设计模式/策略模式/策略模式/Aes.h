#include "Encode.h"

class Aes :public Encode
{
public:
	virtual void Code(const char* const password)
	{
		printf("AESº”√‹À„∑®:%s\n", password);
	}
protected:
};