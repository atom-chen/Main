#pragma  once
#include "RenderAble.h"

class Ground:public RenderAble
{
public:
	Ground();
	bool Init();
	bool Init(const char* picName);
	void Draw();
	void Destory();
protected:

private:
	float height = -1;
};