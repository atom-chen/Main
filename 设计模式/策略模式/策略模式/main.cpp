#include "Aes.h"
#include "Des.h"


int main()
{
	Encode *encode = new Aes;
	encode->Code("1234567");
	delete encode;

	encode = new Des;
	encode->Code("1234567");
	delete encode;
	return 0;
}