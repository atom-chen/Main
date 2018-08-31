#pragma  once
#include "ggl.h"
#include "Vector3.h"

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
	void Update(float frameTime);
	void Draw();
public:
	void SetTexture(const char* bmpPath);
	void SetAmbientMaterialColor(float r, float g, float b, float a = 1);
	void SetDiffuseMaterialColor(float r, float g, float b, float a = 1);
	void SetSpecularMaterialColor(float r, float g, float b, float a = 1);

	void MoveToLeft(bool isMove);
	void MoveToRight(bool isMove);
	void MoveToTop(bool isMove);
	void MoveToBottom(bool isMove);
	void SetMoveSpeed(float speed);
protected:
private:
	VertexData *m_Vertexes;//顶点数组
	unsigned short *m_Indexes;//顶点索引数组
	int m_IndexCount;//顶点数量
	GLuint m_Texture;//该模型的纹理

	//模型的材质
	float m_AmbientMaterial[4], m_DiffuseMaterial[4], m_SpecularMaterial[4];

	bool m_IsMoveToLeft, m_IsMoveToRight, m_IsMoveToTop, m_IsMoveToBottom;
	float m_MoveSpeed = 5;
	Vector3 m_Position{0,0,-5};
};