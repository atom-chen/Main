#pragma once
#include "RenderAble_Light.h"

//SIMPLE版本
class SeaWater:public Node
{
public:
	virtual bool Init(const char* seaTexture,const char* foamTexture=nullptr, const char* lightTexture=nullptr);
	void Update();
	void Draw(glm::mat4& viewMatrix, glm::mat4 &ProjectionMatrix);
	void Destroy();
public:
	void SetSeaSize(float width, float height);//设置海水面积
	void SetWaveHeight(float height);//浪高
	void SetWaveMoveSpeed(float speed);//浪的移动速度
	void SetSeaTexture(const char* picPath);//海水纹理
	void SetFoamTexture(const char* picPath);//浪花纹理
	void SetLightTexture(const char* picPath);//光照纹理
	void SetLight1(const Light& light);//灯光

	void SetShore_Light(const vec3& color);//岸的明rgb
	void SetShore_Dark(const vec3& color);//岸的暗rgb
	void SetSeq_Light(const vec3& color);//水的明rgb
	void SetSeq_Dark(const vec3& color);//水的暗rgb

protected:
private:
	bool m_IsInit = false; 
	mat4 m_ModelMatrix;
	Shader m_Shader;
	VertexBuffer m_VertexBuf;
};