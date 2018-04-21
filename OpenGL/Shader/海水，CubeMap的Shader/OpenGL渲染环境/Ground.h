#pragma  once
#include "RenderAble_Light.h"

class Ground :public RenderAble_Light
{
public:
	Ground();
	bool Init();
	bool Init(const char* picName);
	virtual void Destory();
protected:

private:
	float height = -1;

};