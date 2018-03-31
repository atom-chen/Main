#pragma  once
#include "ggl.h"

class Sprite
{
public:
	void SetImage(const char* path);
	void SetRect(float x, float y, float widget, float height);
	void Draw();
protected:
private:
	GLuint m_Texture;
	vec2 m_Vertexes[4];
};