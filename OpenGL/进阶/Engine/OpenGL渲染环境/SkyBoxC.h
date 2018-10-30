#pragma once
#include "RenderAble.h"

class SkyBoxC:public RenderAble
{
public:
	bool Init(const char* forwardPath, const char* backPath, const char* topPath, const char* bottomPath, const char* leftPath, const char* rightPath,
		const char* vertexShader = SHADER_ROOT"SkyBoxC.vert", const char* fragShader = SHADER_ROOT"SkyBoxC.frag");
	void Update(const vec3& cameraPos);
	virtual void Destroy();
protected:
private:
	string m_ModelName;
};