#pragma once
#include "RenderAble.h"

class SkyBoxC:public RenderAble
{
public:
	bool Init(const char* forwardPath, const char* backPath, const char* topPath, const char* bottomPath, const char* leftPath, const char* rightPath,
		const char* vertexShader = "res/SkyBoxC.vert", const char* fragShader = "res/SkyBoxC.frag");
	void Update(const vec3& cameraPos);
	virtual void Destory();
protected:
private:
	string m_ModelName;
};