#pragma  once
#include "ggl.h"
#include "Node.h"
#include "VertexBuffer.h"
#include "Shader.h"
#include "Transform.h"

//绘制什么类型
enum DRAW_TYPE
{
	DRAW_TRIANGLES,
	DRAW_TRIANGLES_STRIP,
	DRAW_POINT,
	DRAW_QUADS,
};
//绘制选项
struct RenderOption
{
	DRAW_TYPE DrawType = DRAW_TRIANGLES;
	bool DepthTest=0;
	bool AlphaBlend=0;
	bool Program_Point_Size = 0;
};
//需要被渲染的对象继承此类
class RenderAble:public Node
{
public:

protected:
	RenderAble();
	virtual ~RenderAble();
public:
	void SetEnable(bool isDraw){ this->m_IsDraw = isDraw; }
	bool OnEnable(){ return this->m_IsDraw; }
	VertexBuffer& GetVertexBuffer(){ return m_VertexBuf; }
	Shader& GetShader(){ return m_Shader; }
	mat4& GetModelMatrix(){ return m_ModelMatrix; }
public:
	inline bool IsDepthTest()const{ return this->m_Options.DepthTest; }
	inline bool IsAlphaBlend()const{ return this->m_Options.AlphaBlend; }
	inline bool IsProgramPointSize() const{ return this->m_Options.Program_Point_Size; }
	inline DRAW_TYPE GetType()const{ return this->m_Options.DrawType; }
protected:
	VertexBuffer m_VertexBuf;
	Shader m_Shader;
	glm::mat4 m_ModelMatrix;
	bool m_IsDraw = 1;

	RenderOption m_Options;//渲染选项
};
