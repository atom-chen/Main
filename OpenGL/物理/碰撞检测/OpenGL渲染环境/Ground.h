#pragma  once
#include "RenderAble_Light.h"

class Ground :public RenderAble_Light
{
public:
	Ground();
	bool Init();
	bool Init(const char* picName);
protected:

private:
	float height = 0;

};