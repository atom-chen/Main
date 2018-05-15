#pragma once
#include "RenderAble_Light.h"

//SIMPLE�汾
class SeaWater:public Node
{
public:
	virtual bool Init(const char* seaTexture,const char* foamTexture=nullptr, const char* lightTexture=nullptr);
	void Update();
	void Draw(glm::mat4& viewMatrix, glm::mat4 &ProjectionMatrix);
	void Destroy();
public:
	void SetSeaSize(float width, float height);//���ú�ˮ���
	void SetWaveHeight(float height);//�˸�
	void SetWaveMoveSpeed(float speed);//�˵��ƶ��ٶ�
	void SetSeaTexture(const char* picPath);//��ˮ����
	void SetFoamTexture(const char* picPath);//�˻�����
	void SetLightTexture(const char* picPath);//��������
	void SetLight1(const Light& light);//�ƹ�

	void SetShore_Light(const vec3& color);//������rgb
	void SetShore_Dark(const vec3& color);//���İ�rgb
	void SetSeq_Light(const vec3& color);//ˮ����rgb
	void SetSeq_Dark(const vec3& color);//ˮ�İ�rgb

protected:
private:
	bool m_IsInit = false; 
	mat4 m_ModelMatrix;
	Shader m_Shader;
	VertexBuffer m_VertexBuf;
};