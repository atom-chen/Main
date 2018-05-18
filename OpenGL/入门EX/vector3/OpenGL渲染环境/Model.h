#pragma  once
#include "ggl.h"

//v开头 顶点数据 vt=texcoord vn=normal 
//f开头 绘制指令-》face，某个面由哪些点组成
//用于存储v数据
struct VertexData
{
	float position[3];//位置信息
	float nromal[3];//法线信息
	float texcoord[2];//uv信息
};

class Model
{
public:
	Model();
	bool Init(const char* path);
	void Draw();
public:
	void SetTexture(const char* bmpPath);
	void SetAmbientMaterialColor(float r, float g, float b, float a = 1);
	void SetDiffuseMaterialColor(float r, float g, float b, float a = 1);
	void SetSpecularMaterialColor(float r, float g, float b, float a = 1);
protected:
private:
	VertexData *m_Vertexes;//顶点数组
	unsigned short *m_Indexes;//顶点索引数组
	int32_t m_IndexCount;//顶点数量
	GLuint m_Texture;//该模型的纹理

	//模型的材质
	float m_AmbientMaterial[4], m_DiffuseMaterial[4], m_SpecularMaterial[4];
};