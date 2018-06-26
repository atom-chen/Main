#pragma  once
#include "RenderAble.h"

class UITexture:public RenderAble
{
public:
	void SetImage(const char* path);
	virtual void Init(float x, float y, float width, float height, const char* picPath, const char* vertShader = "res/texture.vert", const char* fargShader = "res/texture.frag", const char* picNameInShader = "U_Texture_1");
	virtual void Draw();
	void SetSize(float x,float y,float width, float height);
private:
};